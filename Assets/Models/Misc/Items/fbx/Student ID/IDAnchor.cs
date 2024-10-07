using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class IDAnchor : MonoBehaviour
{
    public Transform homePosition;  // Reference to the chest position
    public float returnSpeed = 5f;  // Speed at which the ID returns

    private XRGrabInteractable grabInteractable;
    private bool isReturning = false;

    private void Start()
    {
        // Get the XRGrabInteractable component on this object
        grabInteractable = GetComponent<XRGrabInteractable>();

        // Subscribe to the Select Exited event
        grabInteractable.selectExited.AddListener(OnReleased);
    }

    private void Update()
    {
        // If the ID is supposed to return, move it smoothly to the home position
        if (isReturning)
        {
            transform.position = Vector3.Lerp(transform.position, homePosition.position, Time.deltaTime * returnSpeed);
            transform.rotation = Quaternion.Slerp(transform.rotation, homePosition.rotation, Time.deltaTime * returnSpeed);

            // Stop returning once it's close enough to the home position
            if (Vector3.Distance(transform.position, homePosition.position) < 0.01f)
            {
                isReturning = false;
            }
        }
    }

    // Called when the ID is released from the player's hand
    private void OnReleased(SelectExitEventArgs args)
    {
        isReturning = true;
    }

    private void OnDestroy()
    {
        // Unsubscribe to avoid memory leaks
        grabInteractable.selectExited.RemoveListener(OnReleased);
    }
}
