using UnityEngine;

public class SmoothCameraFollow : MonoBehaviour
{
    public Transform target;        // The target object that the camera will follow
    public float smoothSpeed = 0.125f;  // The speed at which the camera will smooth
    public Vector3 offset;          // The offset distance between the camera and the target

    void LateUpdate()
    {
        if (!target)
        {
            if (!BF_PlayerSnow.Instance)
            {
                return;
            }

            target = BF_PlayerSnow.Instance.transform;
        }

        // Desired position based on the target's position and the offset
        Vector3 desiredPosition = target.position + offset;

        // Smoothly interpolate between the camera's current position and the desired position
        Vector3 smoothedPosition = Vector3.Lerp(transform.position, desiredPosition, smoothSpeed);

        // Update the camera's position
        transform.position = smoothedPosition;
    }
}