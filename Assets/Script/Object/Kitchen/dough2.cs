using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class NextDough : MonoBehaviour
{
    public Vector3 targetScale = new Vector3(140f, 140f, 50f);  // The final scale when tossed in air
    public float scalingTime = 3f;           // Time to fully scale the dough in air
    private float currentScalingTime = 0f;   // Timer to track the scaling progress
    private Vector3 initialScale;            // Initial scale of the dough
    private float scaleProgress = 0f;        // Track scaling progress

    public float spinSpeed = 180f;           // Speed at which the dough spins while in mid-air
    public GameObject nextDough;

    public GameObject nextItem;

    private XRGrabInteractable grabInteractable;  // XR Grab Interactable component
    private Rigidbody doughRigidbody;        // Rigidbody of the dough for physics
    private bool isInAir = false;            // Flag to check if dough is in mid-air
    private bool isSpinning = false;         // Whether the dough is currently spinning
    private MeshRenderer pizza;

    private void Start()
    {
        pizza = GetComponent<MeshRenderer>();
        // Store the initial scale of the dough
        initialScale = transform.localScale;

        // Get the XRGrabInteractable component and Rigidbody
        grabInteractable = GetComponent<XRGrabInteractable>();
        doughRigidbody = GetComponent<Rigidbody>();

        // Freeze Y rotation to keep the dough facing up
        doughRigidbody.constraints = RigidbodyConstraints.FreezeRotationY | RigidbodyConstraints.FreezeRotationX;

        // Subscribe to the grab and release events of XR Grab Interactable
        grabInteractable.selectExited.AddListener(OnRelease);
        grabInteractable.selectEntered.AddListener(OnGrab);
    }

    private void Update()
    {
        // Check if the dough is in the air and not being held by the player
        if (isInAir && isSpinning)
        {
            // Track how long it's been in the air and scale the dough
            currentScalingTime += Time.deltaTime;

            // Calculate the current scale based on the time in the air
            scaleProgress = Mathf.Clamp01(currentScalingTime / scalingTime);
            transform.localScale = Vector3.Lerp(initialScale, targetScale, scaleProgress);

            // Spin the dough on the Z-axis
            SpinDough();

            // Stop scaling when target scale is reached
            if (currentScalingTime >= scalingTime)
            {
                isInAir = false;  // Stop scaling when done
                pizza.enabled = false;
                nextDough.SetActive(true);
                nextItem.SetActive(true);
            }
        }
    }

    // Triggered when the player releases the dough
    private void OnRelease(SelectExitEventArgs args)
    {
        // Reset the scaling timer, but retain the current scale progress
        currentScalingTime = scaleProgress * scalingTime;

        // Check if the dough is moving upward (i.e., tossed in the air)
        if (doughRigidbody.velocity.y > 0.1f)
        {
            isInAir = true;
            isSpinning = true;

            // Add an initial spin only on the Z-axis
            doughRigidbody.angularVelocity = new Vector3(0f, 0f, spinSpeed * Mathf.Deg2Rad);

            // Freeze X and Z position so it only moves vertically
            doughRigidbody.constraints |= RigidbodyConstraints.FreezePositionX | RigidbodyConstraints.FreezePositionZ;
        }
        else
        {
            isInAir = false;
        }
    }

    // Triggered when the player grabs the dough again
    private void OnGrab(SelectEnterEventArgs args)
    {
        // Unfreeze X and Z positions when grabbed
        doughRigidbody.constraints &= ~(RigidbodyConstraints.FreezePositionX | RigidbodyConstraints.FreezePositionZ);
    }

    // Apply spinning logic only on the Z-axis
    private void SpinDough()
    {
        // Manually control the Z rotation to simulate spinning
        transform.Rotate(Vector3.forward, spinSpeed * Time.deltaTime, Space.Self);
    }

    private void OnCollisionEnter(Collision collision)
    {
        // Detect collision with the ground or another object to stop the spin
        if (collision.gameObject)
        {
            isSpinning = false;

            // Optionally stop the Rigidbody's angular velocity to stop physics-based spinning
            doughRigidbody.angularVelocity = Vector3.zero;

            // Unfreeze X and Z positions when it hits the ground
            doughRigidbody.constraints &= ~(RigidbodyConstraints.FreezePositionX | RigidbodyConstraints.FreezePositionZ);
        }
    }
}
