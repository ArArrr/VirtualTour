using UnityEngine;

public class dough1 : MonoBehaviour
{
    public Transform doughParent;             // Parent object of the dough to scale
    public ParticleSystem doughParticles;     // Particle system for the dough
    public Vector3 targetScale = new Vector3(1f, 0.2f, 1f);  // The final scale of the dough
    public float flatteningTime = 5f;         // Time to fully flatten the dough
    private float currentFlatteningTime = 0f; // Timer to track progress

    private bool isFlattening = false;        // Whether the roller is inside the dough trigger
    private Vector3 initialScale;             // The initial scale of the dough

    [Header("Next Narration")]
    public NarrationController nextNarration;  // Reference to the next narration controller

    public GameObject nextDough;

    private void Start()
    {
        // Store the initial scale of the dough parent
        initialScale = doughParent.localScale;

        // Ensure particle system is stopped initially
        doughParticles.Stop();
    }

    private void Update()
    {
        if (isFlattening)
        {
            // Increment the timer if the roller is inside the dough trigger
            currentFlatteningTime += Time.deltaTime;

            // Calculate the current scale based on how much time has passed
            float progress = Mathf.Clamp01(currentFlatteningTime / flatteningTime);
            doughParent.localScale = Vector3.Lerp(initialScale, targetScale, progress);

            // Stop flattening when target scale is reached
            if (currentFlatteningTime >= flatteningTime)
            {
                isFlattening = false; // Stop flattening when the time is complete
                GetComponent<Collider>().enabled = false; // Disable the collider
                doughParticles.Stop();
                gameObject.SetActive(false);
                

                if (nextNarration != null)
                {
                    nextNarration.StartNarration();
                    nextDough.SetActive(true);
                } else
                {
                    nextDough.SetActive(true);
                }
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        // Check if the roller enters the trigger collider
        if (other.CompareTag("Roller"))
        {
            isFlattening = true;
            doughParticles.Play();  // Start the particle system
        }
    }

    private void OnTriggerExit(Collider other)
    {
        // Check if the roller exits the trigger collider
        if (other.CompareTag("Roller"))
        {
            isFlattening = false;  // Pause the flattening process
            doughParticles.Stop(); // Stop the particle system
        }
    }
}
