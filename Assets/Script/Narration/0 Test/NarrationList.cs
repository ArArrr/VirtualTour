using UnityEngine;

public class NarrationsList : MonoBehaviour
{
    private string playerStrand;
    [Header("Strand / Course")]
    public string requiredStrand = "BSIT";  // Set the required strand in the inspector

    private NarrationController[] narrationControllers;

    private void Start()
    {
        playerStrand = DataManager.Instance.strand;
        // Find all child objects with NarrationController components
        narrationControllers = GetComponentsInChildren<NarrationController>();

        // Check if the player's strand matches the required one
        if (playerStrand.ToLower().Equals(requiredStrand.ToLower())  || requiredStrand.ToLower().Equals("all"))
        {
            Debug.Log("Player's strand matches. Starting narrations.");
            // Enable or start each narration controller
            foreach (var controller in narrationControllers)
            {
                controller.enabled = true;  // Allow the child objects to run
            }
        }
        else
        {
            Debug.Log("Player's strand does not match. Narrations will not start.");
            // Optionally disable the narration controllers if strands don't match
            foreach (var controller in narrationControllers)
            {
                controller.enabled = false;
            }
        }
    }
}
