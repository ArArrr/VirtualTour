using UnityEngine;
using UnityEngine.InputSystem;

public class XRMovementController : MonoBehaviour
{
    public Transform xrOrigin;        // The XR Origin or Camera Offset to move
    public float moveSpeed = 1f;      // Movement speed
    public float verticalSpeed = 1f;  // Vertical movement speed (for E and Q)

    private Vector2 moveInput = Vector2.zero;
    private float verticalInput = 0f;

    private void Update()
    {
        // Apply the movement input to the XR Origin or Camera Offset
        Vector3 moveDirection = new Vector3(moveInput.x, 0, moveInput.y) * moveSpeed * Time.deltaTime;
        Vector3 verticalMove = Vector3.up * verticalInput * verticalSpeed * Time.deltaTime;

        // Move the XR Origin
        xrOrigin.Translate(moveDirection + verticalMove);
    }

    public void OnMove(InputAction.CallbackContext context)
    {
        moveInput = context.ReadValue<Vector2>();
    }

    public void OnVerticalMove(InputAction.CallbackContext context)
    {
        verticalInput = context.ReadValue<float>();
    }
}
