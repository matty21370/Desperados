using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Script created by: Matthew Burke
/// </summary>
public class CameraMovement : MonoBehaviour
{
    /// <summary>
    /// This is the minimum angle we want the camera to have
    /// </summary>
    private const float Y_ANGLE_MIN = -50.0f;

    /// <summary>
    /// This is the maximum angle we want the camera to have
    /// </summary>
    private const float Y_ANGLE_MAX = 50.0f;

    /// <summary>
    /// This is a reference to the object we want to camera to look at
    /// </summary>
    public Transform lookAt;

    /// <summary>
    /// This is a reference to the cameras transform
    /// </summary>
    public Transform camTransform;

    /// <summary>
    /// This is a reference to the camera component attached to the camera object
    /// </summary>
    private Camera cam;

    public bool camEnabled = true;

    private float distance = 10.0f;
    private float currentX = 0.0f;
    private float currentY = 0.0f;
    private float sensivityX = 6.0f;
    private float sensivityY = 3.0f;

    private float minFov = 80;
    private float maxFov = 90;

    private void Start()
    {
        camTransform = transform;
        cam = GetComponent<Camera>();
    }

    private void Update()
    {
            currentX += Input.GetAxis("Mouse X");
            currentY += Input.GetAxis("Mouse Y");

            currentY = Mathf.Clamp(currentY, Y_ANGLE_MIN, Y_ANGLE_MAX);

            if (cam.fieldOfView <= minFov)
            {
                cam.fieldOfView = minFov;
            }
            else if (cam.fieldOfView >= maxFov)
            {
                cam.fieldOfView = maxFov;
            }
    }

    private void LateUpdate()
    {
            Vector3 dir = new Vector3(0, 0, -distance);
            Quaternion rotation = Quaternion.Euler(currentY, currentX, 0);
            camTransform.position = lookAt.position + rotation * dir;

            camTransform.LookAt(lookAt.position);
    }

    public void SetMaxFOV(float n)
    {
        maxFov = n;
    } 
}
