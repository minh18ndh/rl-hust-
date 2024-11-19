using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    [Header("Follow Settings")]
    public Transform target;       // The player (car) to follow
    public float smoothSpeed = 0.125f; // Camera follow smoothness
    public Vector3 offset;         // Offset to maintain distance from the car

    private void LateUpdate()
    {
        if (target == null) return;

        // Target position with offset
        Vector3 desiredPosition = target.position + offset;

        // Smoothly interpolate to the target position
        Vector3 smoothedPosition = Vector3.Lerp(transform.position, desiredPosition, smoothSpeed);

        // Apply the position to the camera
        transform.position = smoothedPosition;
    }
}
