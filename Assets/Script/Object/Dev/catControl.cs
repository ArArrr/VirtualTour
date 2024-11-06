using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.XR.Interaction.Toolkit;

[RequireComponent(typeof(Rigidbody))]
public class LeftRightController : MonoBehaviour
{
    [Tooltip("Input action for the left joystick.")]
    public InputActionProperty leftJoystickInputAction;
    public InputActionProperty pcMovement;
    public float moveSpeed = 1f;
    public float groundDistance = 0.2f;

    private Rigidbody rb;
    private Animator animator;
    public AudioSource audioSource;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
        // Freeze the Z position for 2D movement
        rb.constraints = RigidbodyConstraints.FreezePositionZ | RigidbodyConstraints.FreezeRotation;
        animator = GetComponent<Animator>();
        if (DataManager.Instance.togglePC)
        {
            leftJoystickInputAction = pcMovement;
        }
        if (audioSource == null) audioSource = GetComponent<AudioSource>();
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

        if (animator != null && animator.enabled)
        {
            // Update animator's isWalking parameter based on movement
            animator.SetBool("isWalking", Mathf.Abs(moveDirection) > 0.01f);
            //if (Mathf.Abs(moveDirection) > 0.01f)
            //{
            //    bool isGround = Physics.Raycast(transform.position, Vector3.down, groundDistance);
            //    if (isGround)
            //    {
            //        if (!audioSource.isPlaying) audioSource.Play();
            //        Debug.Log("Playing Walk");
            //    }
            //    else
            //    {
            //        audioSource.Stop();
            //        Debug.Log("Stopped Walk");
            //    }
            //}
            
        }

        // Flip the object's scale based on movement direction
        if (moveDirection > 0.01f)
        {
            transform.localScale = new Vector3(1, transform.localScale.y, transform.localScale.z);
            
        }
        else if (moveDirection < -0.01f)
        {
            transform.localScale = new Vector3(-1, transform.localScale.y, transform.localScale.z);
        }
    }
}
