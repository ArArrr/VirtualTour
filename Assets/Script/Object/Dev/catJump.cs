using UnityEngine;
using UnityEngine.InputSystem;

public class CatJump : MonoBehaviour
{
    [Tooltip("Input action for the A Button.")]
    public InputActionProperty aButton;

    [Tooltip("Jump force for the cat.")]
    public float jumpForce = 5f;

    private Rigidbody rb;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();

    }

    private void OnEnable()
    {
        // Enable the A button action and subscribe to it
        aButton.action.Enable();
        aButton.action.performed += OnJump;
    }

    private void OnDisable()
    {
        // Unsubscribe from the action
        aButton.action.performed -= OnJump;
        aButton.action.Disable();
    }

    private void OnJump(InputAction.CallbackContext context)
    {
        // Only apply jump if the object is grounded
        if (IsGrounded())
        {
            rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
            Debug.Log("Jumping!");
        }
    }

    private bool IsGrounded()
    {
        // Simple check to see if the object is on the ground
        return Physics.Raycast(transform.position, Vector3.down, 0.1f);
    }
}
