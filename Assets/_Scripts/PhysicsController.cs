using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PhysicsController : MonoBehaviour
{
    [SerializeField]
    private GameObject soccerBall;
    [SerializeField]
    private LineRenderer trajectory;
    [SerializeField]
    private Slider fillBar;
    [SerializeField]
    private float fillAmountScalar;

    private Animator tAnimator;
    private Vector3 startPosition = Vector3.zero;
    private Vector3 playerStartPos = Vector3.zero;
    private Rigidbody soccerRigid;

    void Start()
    {
        tAnimator = GetComponent<Animator>();
        soccerRigid = soccerBall.GetComponent<Rigidbody>();
        startPosition = soccerBall.transform.position;
        playerStartPos = transform.position;
    }

    void Update()
    {
        if (!CameraMovement.cam.cameraMovementEnabled) return;
        if (Input.GetMouseButtonDown(0))
        {
            fillBar.value = 0;
        }
        if (Input.GetMouseButton(0))
        {
            fillBar.value += fillAmountScalar * Time.deltaTime;
        }
        if(Input.GetMouseButtonUp(0))
        {
            soccerBall.transform.position = startPosition;
            transform.position = playerStartPos;
            soccerRigid.velocity = Vector3.zero;
            tAnimator.Play("KickSoccer");

            CameraMovement.cam.MoveToShowView();
            CameraMovement.cam.disableCamMovement = true;
        }
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            Application.Quit();
        }
    }

    public void KickSoccerBall()
    {
        Vector3 finalVel = Vector3.zero;
        float pitch = Mathf.Abs(CameraMovement.cam.pitch);
        float yaw = Mathf.Abs(CameraMovement.cam.yaw);

        finalVel.x = Mathf.Sin(yaw * Mathf.Deg2Rad) * Mathf.Cos(pitch * Mathf.Deg2Rad) * fillBar.value;
        finalVel.y = Mathf.Sin(pitch * Mathf.Deg2Rad) * fillBar.value;
        finalVel.z = Mathf.Cos(pitch * Mathf.Deg2Rad) * Mathf.Cos(yaw * Mathf.Deg2Rad) * fillBar.value;

        trajectory.enabled = true;
        trajectory.positionCount = 20;
        float totalTime = (2 * finalVel.y) / Physics.gravity.y;
        float totalTDivisible = totalTime / 20;
        float t = 0;
        for (int i = 0; i < 20; i++)
        {
            float x = -(finalVel.x * t + startPosition.x);
            float y = -(-(Physics.gravity.y * 0.5f) * (t * t) + finalVel.y * t) + startPosition.y;
            float z = -(finalVel.z * t + startPosition.z);
            if (float.IsNaN(x) || float.IsInfinity(x) || float.IsNaN(y) || float.IsInfinity(y)) break;
            trajectory.SetPosition(i, new Vector3(x, y, z));
            t += totalTDivisible;
        }

        soccerRigid.velocity = finalVel;
    }
}
