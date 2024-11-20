using UnityEngine;

public class CameraController : MonoBehaviour
{
    [Header("Follow Settings")]
    public Transform target;             // The player (car) to follow
    public float smoothSpeed = 0.2f;     // Camera follow smoothness
    public Vector3 offset;               // Offset to maintain distance from the car

    [Header("Dynamic Zoom Settings")]
    public Camera cam;                   // Reference to the Camera component
    public float minZoom = 2f;           // Minimum zoom level
    public float maxZoom = 4f;           // Maximum zoom level
    public float zoomSpeed = 0.01f;      // How quickly the zoom adjusts
    public float zoomFactor = 1f;        // Zoom sensitivity to speed

    private Rigidbody2D targetRb;        // Rigidbody2D of the target (car)

    private void Start()
    {
        if (target != null)
        {
            targetRb = target.GetComponent<Rigidbody2D>();
        }

        if (cam == null)
        {
            cam = Camera.main; // Automatically assign the main camera if not set
        }
    }

    private void LateUpdate()
    {
        if (target == null || cam == null || targetRb == null) return;

        FollowTarget();
        AdjustZoom();
    }

    private void FollowTarget()
    {
        // Target position with offset
        Vector3 desiredPosition = target.position + offset;

        // Smoothly interpolate to the target position
        Vector3 smoothedPosition = Vector3.Lerp(transform.position, desiredPosition, smoothSpeed);

        // Apply the position to the camera
        transform.position = smoothedPosition;
    }

    private void AdjustZoom()
    {
        // Get the speed of the car
        float speed = targetRb.velocity.magnitude;

        // Calculate the target zoom level based on the car's speed
        float targetZoom = Mathf.Lerp(minZoom, maxZoom, speed * zoomFactor);

        // Smoothly adjust the camera's orthographic size
        cam.orthographicSize = Mathf.Lerp(cam.orthographicSize, targetZoom, zoomSpeed);
    }
}
