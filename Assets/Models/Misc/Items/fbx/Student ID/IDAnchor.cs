using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class IDAnchor : MonoBehaviour
{
    public Transform homePosition;  // Reference to the chest position
    public float returnSpeed = 5f;  // Speed at which the ID returns

    private XRGrabInteractable grabInteractable;
    private Interactable interactable;
    private bool isReturning = false;

    private void Start()
    {
        grabInteractable = GetComponent<XRGrabInteractable>();
        grabInteractable.selectExited.AddListener(OnReleased);

        interactable = GetComponent<Interactable>();
        if (interactable != null)
        {
            interactable.Dropped += () => Dropped();
        }
    }

    private void Update()
    {
        if (isReturning)
        {
            transform.position = Vector3.Lerp(transform.position, homePosition.position, Time.deltaTime * returnSpeed);
            transform.rotation = Quaternion.Slerp(transform.rotation, homePosition.rotation, Time.deltaTime * returnSpeed);

            if (Vector3.Distance(transform.position, homePosition.position) < 0.01f)
            {
                isReturning = false;
            }
        }
    }

    private void OnReleased(SelectExitEventArgs args)
    {
        isReturning = true;
    }

    private void Dropped()
    {
        if (interactable.isAnchored)
        {
            isReturning = true;
        }
    }

    private void OnDestroy()
    {
        if (interactable != null)
        {
            interactable.OnInteract -= (offset) => Dropped();
        }

        grabInteractable.selectExited.RemoveListener(OnReleased);
    }
}
