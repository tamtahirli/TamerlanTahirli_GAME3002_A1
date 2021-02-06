using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PhysicsController : MonoBehaviour
{
    [SerializeField]
    private GameObject soccerBall;
    [SerializeField]
    private Vector3 landingPosition = Vector3.zero;
    [SerializeField]
    private float maxHeight = 2;
    [SerializeField]
    private float theta = 0;
    [SerializeField]
    private bool lowAngle;

    private Animator tAnimator;

    private Vector3 startPosition = Vector3.zero;
    private Rigidbody soccerRigid;

    void Start()
    {
        tAnimator = GetComponent<Animator>();
        soccerRigid = soccerBall.GetComponent<Rigidbody>();
        startPosition = soccerBall.transform.position;
    }

    void Update()
    {
        if(Input.GetKeyDown(KeyCode.Mouse0))
        {
            soccerBall.transform.position = startPosition;
            soccerRigid.velocity = Vector3.zero;
            tAnimator.Play("KickSoccer");
        }

        if(Input.GetKey(KeyCode.UpArrow))
        {
            theta = Mathf.Clamp(theta + 1, 0, 80);
        }

        if (Input.GetKey(KeyCode.DownArrow))
        {
            theta = Mathf.Clamp(theta - 1, 0, 80);
        }
    }

    public void KickSoccerBall()
    {
        // end height
        float height = soccerBall.transform.position.y;
        // mag of pos - pos
        float distance = Vector3.Distance(landingPosition, soccerBall.transform.position);
        // tan-1(4h / dist)
        float tTheta = Mathf.Atan((4 * height) / (distance));
        // vel mag sqrt(2gh) / sin(theta)

        if (theta > 0)
        {
            tTheta = theta;
        }

        float velocityMagnitude = Mathf.Sqrt((2 * Mathf.Abs(Physics.gravity.y) * height)) / Mathf.Sin(tTheta);

        Vector3 finalVel = Vector3.zero;

        finalVel.y = velocityMagnitude * Mathf.Sin(lowAngle ? -tTheta : tTheta);
        finalVel.x = -(velocityMagnitude * Mathf.Cos(tTheta));

        soccerRigid.velocity = finalVel;
    }
}
