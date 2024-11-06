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
    private Collider collider;
    private Animator animator;
    public bool isActive = true;
    public bool notTriggerWhileDragging = true;

    public LayerMask interactableLayerMask;
    private static VRRaycastUIDrag currentDraggedObject = null;

    private void Start()
    {
        XRRayInteractor[] rayInteractors = FindObjectsOfType<XRRayInteractor>();
        collider = gameObject.GetComponent<Collider>();

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

        pcRaycaster = FindObjectOfType<CameraRaycaster>();
    }

    private void Update()
    {
        if (!isActive) return;

        //Debug.Log(DataManager.Instance.togglePC ? "PC Mode Active" : "VR Mode Active");

        if (DataManager.Instance.togglePC)
        {
            if (Mouse.current.leftButton.isPressed && !isDragging && currentDraggedObject == null)
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

    public void OnPointerDown(PointerEventData eventData)
    {
        if (!isActive) return;
        //Debug.Log("Pointer down detected.");
        StartDragging(eventData);
    }

    private void ManualOnPointerDown()
    {
        if (!isActive) return;
        Ray ray = pcRaycaster.mainCamera.ScreenPointToRay(new Vector3(Screen.width / 2, Screen.height / 2, 0));
        if (Physics.Raycast(ray, out RaycastHit hit, pcRaycaster.raycastDistance, interactableLayerMask) && hit.transform == transform)
        {
            //Debug.Log("Pointer down on valid target in PC Mode.");
            StartDragging(null);
        }
        else
        {
            //Debug.Log("Pointer down but no valid target in PC Mode.");
        }
    }

    private void StartDragging(PointerEventData eventData)
    {
        if (!isActive) return;
        if (currentDraggedObject != null) return;

        isDragging = true;
        currentDraggedObject = this;
        if (notTriggerWhileDragging) collider.excludeLayers = LayerMask.GetMask("UI");

        if (DataManager.Instance.togglePC)
        {
            Ray ray = pcRaycaster.mainCamera.ScreenPointToRay(new Vector3(Screen.width / 2, Screen.height / 2, 0));
            if (Physics.Raycast(ray, out RaycastHit hit, pcRaycaster.raycastDistance, interactableLayerMask))
            {
                offset = transform.position - hit.point;
                //Debug.Log("Dragging started in PC Mode.");
            }
        }
        else
        {
            if (leftRayInteractor != null && leftRayInteractor.TryGetCurrent3DRaycastHit(out RaycastHit leftHit) && leftHit.transform == transform)
            {
                activeRayInteractor = leftRayInteractor;
                offset = transform.position - leftHit.point;
                //Debug.Log("Dragging started in VR Mode with Left Ray Interactor.");
            }
            else if (rightRayInteractor != null && rightRayInteractor.TryGetCurrent3DRaycastHit(out RaycastHit rightHit) && rightHit.transform == transform)
            {
                activeRayInteractor = rightRayInteractor;
                offset = transform.position - rightHit.point;
                //Debug.Log("Dragging started in VR Mode with Right Ray Interactor.");
            }
            else
            {
                //Debug.Log("Pointer down but no valid target in VR Mode.");
            }
        }
    }

    public void OnDrag(PointerEventData eventData)
    {
        if (!isActive) return;
        //Debug.Log("OnDrag event triggered.");
        UpdateDragPosition();
    }

    private void ManualOnDrag()
    {
        if (!isActive) return;
        //Debug.Log("Manual drag in PC Mode.");
        UpdateDragPosition();
    }

    private void UpdateDragPosition()
    {
        if (!isActive || !isDragging) return;

        if (DataManager.Instance.togglePC)
        {
            Ray ray = pcRaycaster.mainCamera.ScreenPointToRay(new Vector3(Screen.width / 2, Screen.height / 2, 0));
            if (Physics.Raycast(ray, out RaycastHit hit, pcRaycaster.raycastDistance, interactableLayerMask))
            {
                Vector3 newPosition = hit.point + offset;
                newPosition.z = transform.position.z; // Lock to X and Y axis
                transform.position = newPosition;
                //Debug.Log("Updating position in PC Mode.");
            }
        }
        else if (activeRayInteractor != null)
        {
            if (activeRayInteractor.TryGetCurrent3DRaycastHit(out RaycastHit hit))
            {
                Vector3 newPosition = hit.point + offset;
                newPosition.z = transform.position.z; // Lock to X and Y axis
                transform.position = newPosition;
                //Debug.Log("Updating position in VR Mode.");
            }
        }
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        if (!isActive) return;
        //Debug.Log("Pointer up detected.");
        StopDragging();
    }

    private void ManualOnPointerUp()
    {
        if (!isActive) return;
        //Debug.Log("Manual pointer up in PC Mode.");
        StopDragging();
    }

    private void StopDragging()
    {
        isDragging = false;
        activeRayInteractor = null;
        currentDraggedObject = null;
        collider.excludeLayers = 0;
        //Debug.Log("Stopped dragging UI element.");
    }
}
