using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine.InputSystem;

public class VRRaycastUIDrag : MonoBehaviour, IPointerDownHandler, IDragHandler, IPointerUpHandler
{
    private XRRayInteractor leftRayInteractor;
    private XRRayInteractor rightRayInteractor;
    private XRRayInteractor activeRayInteractor;
    private CameraRaycaster pcRaycaster;
    private bool isDragging = false;
    private Vector3 offset;

    private void Start()
    {
        // Automatically find Left and Right XRRayInteractors
        XRRayInteractor[] rayInteractors = FindObjectsOfType<XRRayInteractor>();

        foreach (XRRayInteractor interactor in rayInteractors)
        {
            if (interactor.name.Contains("Left Grab Ray"))
            {
                leftRayInteractor = interactor;
            }
            else if (interactor.name.Contains("Right Grab Ray"))
            {
                rightRayInteractor = interactor;
            }
        }

        if (leftRayInteractor == null || rightRayInteractor == null)
        {
            Debug.LogWarning("Could not find both Left and Right XRRayInteractors.");
        }

        // Find CameraRaycaster for PC mode
        pcRaycaster = FindObjectOfType<CameraRaycaster>();
    }

    private void Update()
    {
        if (DataManager.Instance.togglePC)
        {
            // Check for left mouse button press in PC mode
            if (Mouse.current.leftButton.isPressed && !isDragging)
            {
                ManualOnPointerDown();
            }
            else if (isDragging && Mouse.current.leftButton.isPressed)
            {
                ManualOnDrag();
            }
            else if (isDragging && !Mouse.current.leftButton.isPressed)
            {
                ManualOnPointerUp();
            }
        }
    }

    // Called automatically by Unity in VR mode, or manually in PC mode
    public void OnPointerDown(PointerEventData eventData)
    {
        StartDragging(eventData);
    }

    private void ManualOnPointerDown()
    {
        // Simulate OnPointerDown in PC mode
        StartDragging(null);
    }

    private void StartDragging(PointerEventData eventData)
    {
        if (DataManager.Instance.togglePC)
        {
            // PC Mode
            Debug.Log("PC Mode: Starting drag on UI element");
            isDragging = true;
            Ray ray = pcRaycaster.mainCamera.ScreenPointToRay(new Vector3(Screen.width / 2, Screen.height / 2, 0));
            if (Physics.Raycast(ray, out RaycastHit hit, pcRaycaster.raycastDistance, pcRaycaster.interactableLayerMask))
            {
                offset = transform.position - hit.point;
            }
        }
        else
        {
            // VR Mode
            Debug.Log("VR Mode: Starting drag on UI element");
            if (leftRayInteractor != null && leftRayInteractor.TryGetCurrent3DRaycastHit(out RaycastHit leftHit) && leftHit.transform == transform)
            {
                activeRayInteractor = leftRayInteractor;
                offset = transform.position - leftHit.point;
                isDragging = true;
            }
            else if (rightRayInteractor != null && rightRayInteractor.TryGetCurrent3DRaycastHit(out RaycastHit rightHit) && rightHit.transform == transform)
            {
                activeRayInteractor = rightRayInteractor;
                offset = transform.position - rightHit.point;
                isDragging = true;
            }
        }
    }

    // Called automatically by Unity in VR mode, or manually in PC mode
    public void OnDrag(PointerEventData eventData)
    {
        UpdateDragPosition();
    }

    private void ManualOnDrag()
    {
        // Simulate OnDrag in PC mode
        UpdateDragPosition();
    }

    private void UpdateDragPosition()
    {
        if (isDragging)
        {
            if (DataManager.Instance.togglePC)
            {
                // PC Mode: Drag UI with mouse control
                Ray ray = pcRaycaster.mainCamera.ScreenPointToRay(new Vector3(Screen.width / 2, Screen.height / 2, 0));
                if (Physics.Raycast(ray, out RaycastHit hit, pcRaycaster.raycastDistance, pcRaycaster.interactableLayerMask))
                {
                    Vector3 newPosition = hit.point + offset;
                    newPosition.z = transform.position.z; // Lock to X and Y axis
                    transform.position = newPosition;
                }
            }
            else if (activeRayInteractor != null)
            {
                // VR Mode: Drag UI with XRRayInteractor
                if (activeRayInteractor.TryGetCurrent3DRaycastHit(out RaycastHit hit))
                {
                    Vector3 newPosition = hit.point + offset;
                    newPosition.z = transform.position.z; // Lock to X and Y axis
                    transform.position = newPosition;
                }
            }
        }
    }

    // Called automatically by Unity in VR mode, or manually in PC mode
    public void OnPointerUp(PointerEventData eventData)
    {
        StopDragging();
    }

    private void ManualOnPointerUp()
    {
        // Simulate OnPointerUp in PC mode
        StopDragging();
    }

    private void StopDragging()
    {
        // Stop dragging for both VR and PC modes
        isDragging = false;
        activeRayInteractor = null;
        Debug.Log("Stopped dragging UI element");
    }
}
