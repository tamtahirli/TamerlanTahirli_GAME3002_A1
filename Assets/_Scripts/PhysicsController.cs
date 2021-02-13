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
    [SerializeField]
    private int trajectoryLimit = 20;

    private Animator tAnimator;
    private Vector3 startPosition = Vector3.zero;
    private Vector3 playerStartPos = Vector3.zero;
    private Quaternion playerStartRot;
    private Rigidbody soccerRigid;

    void Start()
    {
        tAnimator = GetComponent<Animator>();
        soccerRigid = soccerBall.GetComponent<Rigidbody>();
        startPosition = soccerBall.transform.position;
        playerStartPos = transform.position;
        playerStartRot = transform.rotation;
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
            // Reset soccer ball and player
            soccerBall.transform.position = startPosition;
            transform.position = playerStartPos;
            transform.rotation = playerStartRot;
            soccerRigid.velocity = Vector3.zero;

            // Play animation
            tAnimator.Play("KickSoccer");

            // Move camera to show view. Can still use power bar.
            CameraMovement.cam.MoveToShowView();
            CameraMovement.cam.disableCamMovement = true;
        }
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            Application.Quit();
        }
    }

    // Called at a certain time of kicking animation
    public void KickSoccerBall()
    {
        Sound.Instance.PlaySound(Sound.Instance.kick);
        Vector3 finalVel = Vector3.zero;
        float pitch = Mathf.Abs(CameraMovement.cam.pitch);
        float yaw = Mathf.Abs(CameraMovement.cam.yaw);

        // Calculate forward of camera based on pitch/yaw set from before they shot
        finalVel.x = Mathf.Sin(yaw * Mathf.Deg2Rad) * Mathf.Cos(pitch * Mathf.Deg2Rad) * fillBar.value;
        finalVel.y = Mathf.Sin(pitch * Mathf.Deg2Rad) * fillBar.value;
        finalVel.z = Mathf.Cos(pitch * Mathf.Deg2Rad) * Mathf.Cos(yaw * Mathf.Deg2Rad) * fillBar.value;


        // Calculate trajectory
        trajectory.enabled = true;
        trajectory.positionCount = trajectoryLimit;
        // 2tVy / g
        float totalTime = (2 * finalVel.y) / Physics.gravity.y;
        float totalTDivisible = totalTime / trajectoryLimit;
        float t = 0;

        for (int i = 0; i < trajectoryLimit; i++)
        {
            //Vxt + x0
            float x = -(finalVel.x * t + startPosition.x);
            // 1/2g * t^2 + Vyt + y0
            float y = -(-(Physics.gravity.y * 0.5f) * (t * t) + finalVel.y * t) + startPosition.y;
            //Vzt + z0
            float z = -(finalVel.z * t + startPosition.z);
            if (float.IsNaN(x) || float.IsInfinity(x) || float.IsNaN(y) || float.IsInfinity(y)) break;
            trajectory.SetPosition(i, new Vector3(x, y, z));
            t += totalTDivisible;
        }

        soccerRigid.velocity = finalVel;
    }
}
