using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CameraMovement : MonoBehaviour
{
    [SerializeField]
    private GameObject target = null;
    [SerializeField]
    private float cameraSpeed = 0.5f;
    [SerializeField]
    private float upDist = 5;
    [SerializeField]
    private float behindDist = 2;
    [SerializeField]
    private float rightDist = 2;
    [SerializeField]
    private float mouseRotationSpeed = 2f;
    [SerializeField]
    private Vector2 minMaxYaw = new Vector2(235, 300);
    [SerializeField]
    private Text scoreText;

    public int score = 0;

    public bool cameraMovementEnabled = false;
    public bool disableCamMovement = false;
    public float yaw = 0.0f;
    public float pitch = 45.0f;

    public Vector3 forward = Vector3.zero;

    public static CameraMovement cam;

    private void Awake()
    {
        cam = this;
    }

    void Update()
    {
        if (!cameraMovementEnabled)
        {
            Vector3 camPos = (target.transform.forward * -behindDist) + (target.transform.up * upDist) + (target.transform.right * rightDist);
            transform.position = Vector3.Lerp(transform.position, camPos, cameraSpeed * Time.deltaTime);
            transform.LookAt(target.transform.position);
            if (Vector3.Distance(target.transform.position, transform.position) <= 0.9f)
            {
                showViewPos = transform.position;
                showViewRot = transform.rotation;
                transform.position = new Vector3(0.3f, 1, 0);
                cameraMovementEnabled = true;
                pitch = transform.eulerAngles.x;
                yaw = transform.eulerAngles.y;
            }
        }
        else
        {
            if (disableCamMovement)
            {
                if (Input.GetKeyDown(KeyCode.R))
                {
                    MoveToShootView();
                }
                return;
            }
            forward = transform.forward;
            lastPos = transform.position;
            lastRot = transform.rotation;
            yaw += mouseRotationSpeed * Input.GetAxis("Mouse X");
            pitch -= mouseRotationSpeed * Input.GetAxis("Mouse Y");
            pitch = Mathf.Clamp(pitch, -65, 0);
            yaw = Mathf.Clamp(yaw, minMaxYaw.x, minMaxYaw.y);
            transform.eulerAngles = new Vector3(pitch, yaw, 0.0f);
        }
    }

    private Vector3 showViewPos;
    private Quaternion showViewRot;
    private Vector3 lastPos;
    private Quaternion lastRot;

    public void MoveToShowView()
    {
        transform.position = showViewPos;
        transform.rotation = showViewRot;
    }

    public void MoveToShootView()
    {
        transform.position = lastPos;
        transform.rotation = lastRot;
        disableCamMovement = false;
    }

    public void UpdateScore(int val)
    {
        score += val;
        scoreText.text = "Score: " + score;
    }
}
