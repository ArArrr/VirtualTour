using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.XR.Interaction.Toolkit;

[RequireComponent(typeof(Rigidbody))]
public class LeftRightController : MonoBehaviour
{
    [Tooltip("Input action for the left joystick.")]
    public InputActionProperty leftJoystickInputAction;

    public float moveSpeed = 1f;

    private Rigidbody rb;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
        // Freeze the Z position for 2D movement
        rb.constraints = RigidbodyConstraints.FreezePositionZ | RigidbodyConstraints.FreezeRotation;
    }

    private void OnEnable()
    {
        leftJoystickInputAction.action.Enable();
    }

    private void OnDisable()
    {
        leftJoystickInputAction.action.Disable();
    }

    private void FixedUpdate()
    {
        // Get joystick input
        Vector2 joystickValue = leftJoystickInputAction.action.ReadValue<Vector2>();

        // Apply only the horizontal (X) input for left-right movement
        float moveDirection = joystickValue.x;

        // Calculate the movement
        Vector3 movement = new Vector3(moveDirection * moveSpeed * Time.fixedDeltaTime, 0, 0);

        // Move the Rigidbody
        rb.MovePosition(transform.position + movement);
    }
}
