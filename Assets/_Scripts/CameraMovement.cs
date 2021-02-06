using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
    private float lookSensitivity = 0.2f;

    // simple script to follow character from behind

    void Update()
    {
        Vector3 camPos = (target.transform.forward * -behindDist) + (target.transform.up * upDist) + (target.transform.right * rightDist);
        transform.position = Vector3.Lerp(transform.position, camPos, cameraSpeed * Time.deltaTime);
        transform.LookAt(target.transform.position);
    }
}
