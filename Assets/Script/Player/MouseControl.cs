using UnityEngine;
using UnityEngine.InputSystem;

public class MouseControl : MonoBehaviour
{
    public InputActionReference lookAction;  // Drag your "Look" action from the Input Actions asset here

    public float sensitivity = 60f;
    public Transform playerBody;  // Assign the player body to rotate left and right
    private float xRotation = 0f;

    private void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
    }

    private void OnEnable()
    {
        // Enable the action
        Cursor.lockState = CursorLockMode.Locked;
        lookAction.action.Enable();
    }

    private void OnDisable()
    {
        // Disable the action
        Cursor.lockState = CursorLockMode.None;
        lookAction.action.Disable();
    }

    private void Update()
    {
        // Read the mouse input from the InputAction
        Vector2 mouseInput = lookAction.action.ReadValue<Vector2>();

        // Calculate mouse movement
        float mouseX = mouseInput.x * sensitivity * Time.deltaTime;
        float mouseY = mouseInput.y * sensitivity * Time.deltaTime;

        // Adjust xRotation to clamp vertical rotation between -90 and +90 degrees
        xRotation -= mouseY;
        xRotation = Mathf.Clamp(xRotation, -90f, 90f);

        // Apply the rotations
        transform.localRotation = Quaternion.Euler(xRotation, 0f, 0f); // Vertical rotation
        playerBody.Rotate(Vector3.up * mouseX); // Horizontal rotation
    }
}
