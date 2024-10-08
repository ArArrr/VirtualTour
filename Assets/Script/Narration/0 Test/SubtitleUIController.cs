using UnityEngine;

public class SubtitleUIController : MonoBehaviour
{
    public Transform targetCamera;          // The camera (usually your VR camera)
    public float distanceFromCamera = 2f;   // Adjustable distance in the inspector
    public float smoothSpeed = 5f;          // Smooth transition speed
    public float updateAngleThreshold = 8.5f; // Threshold for angle before updating
    public float updateDistanceThreshold = 0.5f; // Threshold for distance before updating

    private Vector3 targetPosition;         // The position where the UI should be
    private bool shouldUpdate = false;      // Should update position

    private void Start()
    {
        if (targetCamera == null)
        {
            targetCamera = Camera.main.transform; // Automatically find the main camera if none assigned
        }
    }

    private void LateUpdate()
    {
        // Calculate the target position in front of the camera
        targetPosition = targetCamera.position + targetCamera.forward * distanceFromCamera;

        // Check the angle between the camera and the UI
        Vector3 directionToUI = transform.position - targetCamera.position;
        float angleToUI = Vector3.Angle(targetCamera.forward, directionToUI.normalized);

        // Check the distance between the camera and the UI
        float distanceToUI = Vector3.Distance(transform.position, targetCamera.position);

        // Update if the angle exceeds the threshold or the distance is too large
        if (angleToUI > updateAngleThreshold || Mathf.Abs(distanceToUI - distanceFromCamera) > updateDistanceThreshold)
        {
            shouldUpdate = true;
        }
        else
        {
            shouldUpdate = false;
        }

        // Smoothly transition the UI to the target position if necessary
        if (shouldUpdate)
        {
            transform.position = Vector3.Lerp(transform.position, targetPosition, smoothSpeed * Time.deltaTime);

            // Optionally, make the UI always face the camera
            transform.rotation = Quaternion.LookRotation(transform.position - targetCamera.position);
        }
    }
}
