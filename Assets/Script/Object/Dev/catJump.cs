using UnityEngine;
using UnityEngine.InputSystem;

public class CatJump : MonoBehaviour
{
    [Tooltip("Input action for the A Button.")]
    public InputActionProperty aButton;

    [Tooltip("Jump force for the cat.")]
    public float jumpForce = 5f;

    public float groundDistance = 0.1f;

    [Header("Audio")]
    [Tooltip("Jump sound for the cat.")]
    public AudioClip jumpSound;
    public AudioSource source;


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
            source.clip = jumpSound;
            source.Play();
            Debug.Log("Jumping!");
        }
    }

    private bool IsGrounded()
    {
        bool isGround = Physics.Raycast(transform.position, Vector3.down, groundDistance);
        Debug.Log("Ground is " + isGround);
        // Simple check to see if the object is on the ground
        return isGround;
        
    }
}
