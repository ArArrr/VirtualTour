using System.Collections;
using UnityEngine;

public class SmoothRotationTrigger : MonoBehaviour
{
    [Header("Door")]
    public GameObject targetObject;                 // The primary object to rotate
    public Vector3 targetRotation;                   // The rotation to reach for the primary object
    [Header("Door2 (Optional)")]
    public GameObject secondaryTargetObject;        // Optional second object to rotate
    public Vector3 secondaryTargetRotation;          // The rotation to reach for the secondary object
    public float rotationSpeed = 2f;                 // Speed of rotation

    private Quaternion originalRotation;              // Store the original rotation of the primary target object
    private Quaternion originalSecondaryRotation;     // Store the original rotation of the secondary target object
    private bool isPlayerInside = false;              // Track if the player is inside the trigger
    private float pcCheckDuration = 10f;              // Time to check for PC mode
    private Coroutine pcModeCheckCoroutine;
    private HingeJoint hinge;                    // HingeJoint component of the target object
    private Rigidbody targetRigidbody;                // Rigidbody of the target object
    private Rigidbody secondaryTargetRigidbody;       // Rigidbody of the secondary target object

    [Header("Lock")]
    public bool isLock = false;

    private void Start()
    {
        if (isLock)
        {
            targetRigidbody = targetObject.GetComponent<Rigidbody>();
            targetRigidbody.isKinematic = true;
            gameObject.SetActive(false);
            return;
        }
        // Store the original rotation of the target objects
        originalRotation = targetObject.transform.rotation;
        if (secondaryTargetObject != null)
        {
            originalSecondaryRotation = secondaryTargetObject.transform.rotation;
        }

        // Get the HingeJoint and Rigidbody components from the target object
        hinge = targetObject.GetComponent<HingeJoint>();
        targetRigidbody = targetObject.GetComponent<Rigidbody>();

        // Automatically get the Rigidbody for the secondary target object
        if (secondaryTargetObject != null)
        {
            secondaryTargetRigidbody = secondaryTargetObject.GetComponent<Rigidbody>();
        }

        // Start checking for PC mode
        pcModeCheckCoroutine = StartCoroutine(CheckPCMode());
    }

    private void Update()
    {
        if (DataManager.Instance.togglePC)
        {
            // Disable useSpring for the hinge joint and set the rigidbody to kinematic if in PC mode
            if (hinge != null)
            {
                hinge.useSpring = false;
            }
            if (targetRigidbody != null)
            {
                targetRigidbody.isKinematic = true; // Set the Rigidbody to kinematic
            }
            if (secondaryTargetRigidbody != null)
            {
                secondaryTargetRigidbody.isKinematic = true; // Set the secondary Rigidbody to kinematic
            }

            if (isPlayerInside)
            {
                // Smoothly rotate to the target rotation for the primary object
                Quaternion targetQuat = Quaternion.Euler(targetRotation);
                targetObject.transform.rotation = Quaternion.Slerp(targetObject.transform.rotation, targetQuat, rotationSpeed * Time.deltaTime);

                // If a secondary target exists, rotate it to its specific target rotation
                if (secondaryTargetObject != null)
                {
                    Quaternion secondaryTargetQuat = Quaternion.Euler(secondaryTargetRotation);
                    secondaryTargetObject.transform.rotation = Quaternion.Slerp(secondaryTargetObject.transform.rotation, secondaryTargetQuat, rotationSpeed * Time.deltaTime);
                }
            }
            else
            {
                // Return to the original rotation for the primary object
                targetObject.transform.rotation = Quaternion.Slerp(targetObject.transform.rotation, originalRotation, rotationSpeed * Time.deltaTime);

                // If a secondary target exists, return it to its original rotation as well
                if (secondaryTargetObject != null)
                {
                    secondaryTargetObject.transform.rotation = Quaternion.Slerp(secondaryTargetObject.transform.rotation, originalSecondaryRotation, rotationSpeed * Time.deltaTime);
                }
            }
        }
        else
        {
            // Re-enable useSpring and set the rigidbody to non-kinematic if not in PC mode
            if (hinge != null)
            {
                hinge.useSpring = true;
            }
            if (targetRigidbody != null)
            {
                targetRigidbody.isKinematic = false; // Set the Rigidbody to non-kinematic
            }
            if (secondaryTargetRigidbody != null)
            {
                secondaryTargetRigidbody.isKinematic = false; // Set the secondary Rigidbody to non-kinematic
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        // Check if the player entered the trigger
        if (other.CompareTag("Player"))
        {
            isPlayerInside = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        // Check if the player exited the trigger
        if (other.CompareTag("Player"))
        {
            isPlayerInside = false;
        }
    }

    private IEnumerator CheckPCMode()
    {
        while (true)
        {
            // Wait for the specified duration
            yield return new WaitForSeconds(pcCheckDuration);

            // If not in PC mode, disable this script
            if (!DataManager.Instance.togglePC)
            {
                this.enabled = false;
                break;
            }
        }
    }

    public void unlock()
    {
        isLock = false;
        Start();
    }
}
