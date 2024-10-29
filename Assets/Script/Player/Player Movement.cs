using UnityEngine;
using UnityEngine.InputSystem;

public class CharacterControllerScript : MonoBehaviour
{
    public float walkSpeed = 1.0f;
    public float sprintSpeed = 2.0f;
    public GameObject player;
    public GameObject leftHandController;
    public GameObject rightHandController;
    public Vector3 leftHandPosition = new Vector3(0.0f, 0.0f, 0.0f);
    public Vector3 rightHandPosition = new Vector3(0.0f, 0.0f, 0.0f);

    private CharacterController characterController;
    private Vector2 moveInput;
    private Transform cameraTransform;
    private bool isSprinting = false;

    public InputActionReference moveAction;
    public InputActionReference sprintAction;

    void Start()
    {
        player = GameObject.Find("XR Origin (VR)");
        characterController = player.GetComponent<CharacterController>();
        cameraTransform = Camera.main.transform;

        leftHandController = GameObject.Find("Left Hand Controller");
        rightHandController = GameObject.Find("Right Hand Controller");

        // Enable input actions
        moveAction.action.Enable();
        sprintAction.action.Enable();
    }

    void Update()
    {
        // Adjust hand positions
        if (leftHandController != null)
        {
            leftHandController.transform.localPosition = leftHandPosition;
        }

        if (rightHandController != null)
        {
            rightHandController.transform.localPosition = rightHandPosition;
        }

        // Get the forward direction based on the camera
        Vector3 forward = cameraTransform.forward;
        Vector3 right = cameraTransform.right;

        // Normalize the vectors
        forward.y = 0f;
        right.y = 0f;
        forward.Normalize();
        right.Normalize();

        // Calculate the direction to move
        float speed = isSprinting ? sprintSpeed : walkSpeed;
        Vector3 move = forward * moveInput.y + right * moveInput.x;
        characterController.Move(move * speed * Time.deltaTime);
    }

    // This method is called when the "Move" action is triggered
    public void OnMove(InputAction.CallbackContext context)
    {
        moveInput = context.ReadValue<Vector2>();
    }

    // This method is called when the "Sprint" action is triggered
    public void OnSprint(InputAction.CallbackContext context)
    {
        isSprinting = context.ReadValueAsButton();
    }
}
