using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerInteraction : MonoBehaviour
{
    public float playerReach = 3f;
    private Interactable currentInteractable;

    [Header("Input Actions")]
    public InputActionReference pressE;     // Reference for 'E' key action
    public InputActionReference pressG;     // Reference for 'G' key action
    public InputActionReference mouseClick; // Reference for mouse click action

    [Header("Interaction Settings")]
    public Vector3 offset; // Define the offset from XR Origin
    private Transform xrOrigin;
    private bool isHoldingItem = false; // Track if the player is holding an item

    private void Start()
    {
        // Locate XR Origin (VR)
        GameObject xrOriginObj = GameObject.Find("XR Origin (VR)");
        if (xrOriginObj != null)
        {
            xrOrigin = xrOriginObj.transform;
        }
        else
        {
            Debug.LogError("XR Origin (VR) object not found in the scene.");
        }

        // Enable input actions and bind to methods
        pressE.action.Enable();
        pressG.action.Enable();
        mouseClick.action.Enable();

        pressE.action.performed += context => EKeyInteraction();
        pressG.action.performed += context => GKeyInteraction();
        mouseClick.action.performed += context => ClickInteraction();
    }

    public void getCamera()
    {
        GameObject cameraObj = GameObject.Find("Camera(Clone)");
        if (cameraObj != null)
        {
            Interactable cemaraInt = cameraObj.GetComponent<Interactable>();
            SetNewCurrentInteractable(cemaraInt);
            Debug.Log(currentInteractable.name);
            isHoldingItem = false;
            EKeyInteraction();
            Debug.Log(currentInteractable.name);
        }
        else
        {
            Debug.Log("Camera is null");
        }
    }

    private void OnDestroy()
    {
        // Disable input actions and unbind to avoid memory leaks
        pressE.action.Disable();
        pressG.action.Disable();
        mouseClick.action.Disable();

        pressE.action.performed -= context => EKeyInteraction();
        pressG.action.performed -= context => GKeyInteraction();
        mouseClick.action.performed -= context => ClickInteraction();
    }

    private void Update()
    {
        CheckInteraction();
    }

    private void EKeyInteraction()
    {
        if (currentInteractable != null)
        {
            if (!isHoldingItem)
            {
                // If not holding an item, pick up the item and set isHoldingItem to true
                Debug.Log("Picked up " + currentInteractable.name);
                currentInteractable.Interact(offset);
                isHoldingItem = true;
            }
            else
            {
                // If already holding an item, drop it and reset the state
                Debug.Log("Dropped " + currentInteractable.name);
                currentInteractable.Interact(Vector3.zero); // Detach item by calling Interact again
                DisableCurrentInteractable();
                isHoldingItem = false;
            }
        }
        Debug.Log("No Item selected");
    }

    private void GKeyInteraction()
    {
        if (currentInteractable != null)
        {
            Debug.Log("G key pressed on " + currentInteractable.name);
            // Add additional logic for G key interaction
        }
    }

    private void ClickInteraction()
    {
        if (currentInteractable != null)
        {
            Debug.Log("Mouse clicked on " + currentInteractable.name);
            // Add additional logic for mouse click interaction
        }
    }

    private void CheckInteraction()
    {
        if (isHoldingItem) return; // Skip interaction check if holding an item

        RaycastHit hit;
        Ray ray = new Ray(Camera.main.transform.position, Camera.main.transform.forward);

        // If colliders with anything within player reach
        if (Physics.Raycast(ray, out hit, playerReach))
        {
            if (hit.collider.CompareTag("Camera"))
            {
                DataManager.Instance.cameraInUse = true;
            }
            if (hit.collider.CompareTag("Interactable") || hit.collider.CompareTag("SauceBowl") || hit.collider.CompareTag("IDCard") || hit.collider.CompareTag("Roller") || hit.collider.CompareTag("Camera"))
            {
                Interactable newInteract = hit.collider.GetComponent<Interactable>();
                if (newInteract != null && newInteract.enabled)
                {
                    SetNewCurrentInteractable(newInteract);
                    
                }
                else
                {
                    DisableCurrentInteractable();
                }
            }
        }
        else
        {
            DisableCurrentInteractable();
        }
    }

    private void SetNewCurrentInteractable(Interactable newInteractable)
    {
        if (currentInteractable != newInteractable)
        {
            DisableCurrentInteractable();
            currentInteractable = newInteractable;
            currentInteractable.EnableOutline();
        }
    }

    private void DisableCurrentInteractable()
    {
        if (currentInteractable != null)
        {
            currentInteractable.DisableOutline();
            currentInteractable = null;
        }
    }
}
