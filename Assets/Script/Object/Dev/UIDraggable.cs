using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.XR.Interaction.Toolkit;

public class VRRaycastUIDrag : MonoBehaviour, IPointerDownHandler, IDragHandler
{
    private XRRayInteractor leftRayInteractor;
    private XRRayInteractor rightRayInteractor;
    private XRRayInteractor activeRayInteractor;
    private CameraRaycaster pcRaycaster;

    private Vector3 offset;

    private void Start()
    {
        // Automatically find and assign the Left and Right XRRayInteractors
        XRRayInteractor[] rayInteractors = FindObjectsOfType<XRRayInteractor>();

        foreach (XRRayInteractor interactor in rayInteractors)
        {
            if (interactor.name.Contains("Left"))
            {
                leftRayInteractor = interactor;
            }
            else if (interactor.name.Contains("Right"))
            {
                rightRayInteractor = interactor;
            }
        }

        if (leftRayInteractor == null || rightRayInteractor == null)
        {
            Debug.LogError("Could not find both Left and Right XRRayInteractors.");
        }

        // Find and reference the CameraRaycaster for PC mode
        pcRaycaster = FindObjectOfType<CameraRaycaster>();
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        if (DataManager.Instance.togglePC)
        {
            // PC mode: start dragging with CameraRaycaster
            offset = transform.position - pcRaycaster.mainCamera.ScreenToWorldPoint(
                new Vector3(Screen.width / 2, Screen.height / 2, pcRaycaster.raycastDistance));
        }
        else
        {
            // VR mode: determine which interactor is hitting the UI element
            if (leftRayInteractor != null && leftRayInteractor.TryGetCurrent3DRaycastHit(out RaycastHit leftHit) && leftHit.transform == transform)
            {
                activeRayInteractor = leftRayInteractor;
                offset = transform.position - leftHit.point;
            }
            else if (rightRayInteractor != null && rightRayInteractor.TryGetCurrent3DRaycastHit(out RaycastHit rightHit) && rightHit.transform == transform)
            {
                activeRayInteractor = rightRayInteractor;
                offset = transform.position - rightHit.point;
            }
        }
    }

    public void OnDrag(PointerEventData eventData)
    {
        if (DataManager.Instance.togglePC)
        {
            // PC mode: update position based on camera raycast
            Ray ray = pcRaycaster.mainCamera.ScreenPointToRay(new Vector3(Screen.width / 2, Screen.height / 2, 0));
            if (Physics.Raycast(ray, out RaycastHit hit, pcRaycaster.raycastDistance, pcRaycaster.interactableLayerMask))
            {
                Vector3 newPosition = hit.point + offset;
                newPosition.z = transform.position.z; // Lock to X and Y
                transform.position = newPosition;
            }
        }
        else
        {
            // VR mode: use activeRayInteractor to update position
            if (activeRayInteractor != null && activeRayInteractor.TryGetCurrent3DRaycastHit(out RaycastHit hit))
            {
                Vector3 newPosition = hit.point + offset;
                newPosition.z = transform.position.z; // Lock to X and Y
                transform.position = newPosition;
            }
        }
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        activeRayInteractor = null;
    }
}
