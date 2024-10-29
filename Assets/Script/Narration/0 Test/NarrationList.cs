using UnityEngine;

public class NarrationsList : MonoBehaviour
{
    public enum FloorOption
    {
        Floor8,
        Floor7,
        Floor6,
        Floor5,
        Floor4,
        Floor3,
        Floor2,
        Ground,
        Ground2,
        Basement,
        Complete,
        Baking,
        Photography,
        GameDev
    }

    private string playerStrand;
    private int currentfloor;

    [Header("Strand / Course")]
    public string requiredStrand = "BSIT";  // Set the required strand in the inspector

    [Header("Floor")]
    public FloorOption selectedFloor;
    public bool demoMode = false;

    private NarrationController[] narrationControllers;
    private MarkerDistanceDisplay[] markerDisplays;

    private void Start()
    {
        playerStrand = DataManager.Instance.strand;
        currentFloor();

        // Find all child objects with NarrationController components
        narrationControllers = GetComponentsInChildren<NarrationController>();

        // Find all child objects with MarkerDistanceDisplay components
        markerDisplays = GetComponentsInChildren<MarkerDistanceDisplay>();

        // Check if the player's strand matches the required one and the current floor is valid
        if ((playerStrand.ToLower().Equals(requiredStrand.ToLower()) && currentfloor == DataManager.Instance.lastCompletedFloor) || (requiredStrand.ToLower().Equals("all") && currentfloor == DataManager.Instance.lastCompletedFloor) || demoMode)
        {
            Debug.Log("Player's strand matches " + requiredStrand + ". Starting narrations and marker displays.");

            // Enable or start each narration controller
            foreach (var controller in narrationControllers)
            {
                controller.enabled = true;  // Allow the child objects to run
            }

            // Enable or start each marker display
            foreach (var marker in markerDisplays)
            {
                marker.enabled = true;  // Enable marker functionality
            }
        }
        else
        {
            Debug.Log("Player's strand does not match " + requiredStrand + ". Narrations and marker displays will not start.");
            
            // Optionally disable the narration controllers if strands don't match
            foreach (var controller in narrationControllers)
            {
                controller.enabled = false;
            }

            // Optionally disable marker displays
            foreach (var marker in markerDisplays)
            {
                marker.enabled = false;
            }
            gameObject.SetActive(false);
        }
    }

    void currentFloor()
    {
        switch (selectedFloor)
        {
            case FloorOption.Floor8:    currentfloor = 2; break;
            case FloorOption.Floor7:    currentfloor = 3; break;
            case FloorOption.Floor6:    currentfloor = 4; break;
            case FloorOption.Floor5:    currentfloor = 5; break;
            case FloorOption.Floor4:    currentfloor = 6; break;
            case FloorOption.Floor3:    currentfloor = 7; break;
            case FloorOption.Floor2:    currentfloor = 8; break;
            case FloorOption.Ground:    currentfloor = 0; break;
            case FloorOption.Ground2:   currentfloor = 9; break;
            case FloorOption.Basement:  currentfloor = 1; break;
            case FloorOption.Complete: currentfloor = 10; break;
            case FloorOption.Baking: currentfloor = 11; break;
            case FloorOption.Photography: currentfloor = 12; break;
            case FloorOption.GameDev: currentfloor = 13; break;
        }
    }
    public void setLevel(int level)
    {
        DataManager.Instance.lastCompletedFloor = level;
    }
    public void setTour(bool tour)
    {
        DataManager.Instance.isTour = tour;
    }
    public void setCamp(string c)
    {
        DataManager.Instance.camp = c.ToLower();
    }
    public void toggleNextlevel(bool nxtLevel)
    {
        DataManager.Instance.nextLevel = nxtLevel;
    }
}
