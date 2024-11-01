using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

public class CharacterControllerScript : MonoBehaviour
{
    public float walkSpeed = 1.0f;
    public float sprintSpeed = 2.0f;
    public GameObject player;
    public GameObject CameraOffset;
    public GameObject leftHandController;
    public GameObject rightHandController;
    public Vector3 leftHandPosition = new Vector3(0.0f, 0.0f, 0.0f);
    public Vector3 rightHandPosition = new Vector3(0.0f, 0.0f, 0.0f);
    public float crouchTransitionDuration = 0.3f; // Duration for the transition
    public InputActionReference moveAction;
    public InputActionReference sprintAction;
    public InputActionReference crouchAction; // Reference to the crouch action

    private CharacterController characterController;
    private Vector2 moveInput;
    private Transform cameraTransform;
    private bool isSprinting = false;
    private Vector3 originalCameraOffsetPos;
    private Vector3 crouchedPosition = new Vector3(0, 1f, 0);
    private Coroutine crouchCoroutine;

    void Start()
    {
        CameraOffset = GameObject.Find("Camera Offset");
        player = GameObject.Find("XR Origin (VR)");
        characterController = player.GetComponent<CharacterController>();
        cameraTransform = Camera.main.transform;

        leftHandController = GameObject.Find("Left Hand Controller");
        rightHandController = GameObject.Find("Right Hand Controller");

        // Save the original CameraOffset position
        originalCameraOffsetPos = new Vector3(0, 1.5f, 0);

        // Enable input actions
        moveAction.action.Enable();
        sprintAction.action.Enable();
        crouchAction.action.Enable(); // Enable the crouch action
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

    // This method is called when the "Crouch" action is triggered
    public void OnCrouch(InputAction.CallbackContext context)
    {
        if (context.started)
        {
            // Start crouch transition
            StartCrouchTransition(crouchedPosition);
        }
        else if (context.canceled)
        {
            // Return to standing position
            StartCrouchTransition(originalCameraOffsetPos);
        }
    }

    // Smoothly transition to the target position
    private void StartCrouchTransition(Vector3 targetPosition)
    {
        // Stop any ongoing crouch transition
        if (crouchCoroutine != null)
        {
            StopCoroutine(crouchCoroutine);
        }

        // Start a new transition to the target position
        crouchCoroutine = StartCoroutine(CrouchTransition(targetPosition));
    }

    // Coroutine for a smooth position transition
    private IEnumerator CrouchTransition(Vector3 targetPosition)
    {
        Vector3 startPosition = CameraOffset.transform.localPosition;
        float elapsedTime = 0f;

        while (elapsedTime < crouchTransitionDuration)
        {
            CameraOffset.transform.localPosition = Vector3.Lerp(startPosition, targetPosition, elapsedTime / crouchTransitionDuration);
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        // Ensure the final position is set to the target
        CameraOffset.transform.localPosition = targetPosition;
    }
}
