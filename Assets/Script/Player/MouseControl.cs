using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using UnityEngine.XR.Interaction.Toolkit.UI;

public class MouseControl : MonoBehaviour
{
    public InputActionReference lookAction;  // Drag your "Look" action from the Input Actions asset here
    public InputActionReference esc;         // Drag your "esc" key action from the Input Actions asset here

    public float sensitivity = 60f;
    public Transform playerBody;  // Assign the player body to rotate left and right
    private float xRotation = 0f;
    public bool isActive = true;
    EventSystem eventSystem;
    XRUIInputModule xruiInputModule;

    private void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
    }

    private void OnEnable()
    {
        // Enable actions
        Cursor.lockState = CursorLockMode.Locked;
        lookAction.action.Enable();
        esc.action.Enable();
        eventSystem = FindObjectOfType<EventSystem>();

        // Bind esc key action
        esc.action.performed += context => escKey();
    }

    private void OnDisable()
    {
        // Disable actions and unbind esc key
        Cursor.lockState = CursorLockMode.None;
        lookAction.action.Disable();
        esc.action.Disable();

        esc.action.performed -= context => escKey();
    }

    private void Update()
    {
        if (isActive)
        {
            sensitivity = DataManager.Instance.mouseSensitivity;
            sensitivity = sensitivity / 150;
            // Read the mouse input from the InputAction
            Vector2 mouseInput = lookAction.action.ReadValue<Vector2>();

            // Calculate mouse movement
            float mouseX = mouseInput.x * sensitivity;
            float mouseY = mouseInput.y * sensitivity;

            // Adjust xRotation to clamp vertical rotation between -90 and +90 degrees
            xRotation -= mouseY;
            xRotation = Mathf.Clamp(xRotation, -90f, 90f);

            // Apply the rotations
            transform.localRotation = Quaternion.Euler(xRotation, 0f, 0f); // Vertical rotation
            playerBody.Rotate(Vector3.up * mouseX); // Horizontal rotation
        }
    }

    private void escKey()
    {
        if (isActive)
        {
            Cursor.lockState = CursorLockMode.None;
            isActive = false;
            // Find the EventSystem object and disable mouse input
            
            if (eventSystem != null)
            {
                xruiInputModule = eventSystem.GetComponent<XRUIInputModule>();
                if (xruiInputModule != null)
                {
                    // Set m_EnableMouseInput to false
                    xruiInputModule.m_EnableMouseInput = true;
                }
            }
        }
        else
        {
            Cursor.lockState = CursorLockMode.Locked;
            isActive = true;
            if (eventSystem != null)
            {
                xruiInputModule = eventSystem.GetComponent<XRUIInputModule>();
                if (xruiInputModule != null)
                {
                    // Set m_EnableMouseInput to false
                    xruiInputModule.m_EnableMouseInput = false;
                }
            }
        }
    }
}
