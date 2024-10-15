using DG.Tweening.Plugins.Options;
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
        Basement
    }

    private string playerStrand;
    private int currentfloor;
    [Header("Strand / Course")]
    public string requiredStrand = "BSIT";  // Set the required strand in the inspector

    [Header("Floor")]
    public FloorOption selectedFloor;

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

    void currentFloor()
    {
        switch (selectedFloor)
        {
            case FloorOption.Floor8:
                {
                    currentfloor = 8;
                    break;
                }
            case FloorOption.Floor7:
                {
                    currentfloor = 7;
                    break;
                }
            case FloorOption.Floor6:
                {
                    currentfloor = 6;
                    break;
                }
            case FloorOption.Floor5:
                {
                    currentfloor = 5;
                    break;
                }
            case FloorOption.Floor4:
                {
                    currentfloor = 4;
                    break;
                }
            case FloorOption.Floor3:
                {
                    currentfloor = 3;
                    break;
                }
            case FloorOption.Floor2:
                {
                    currentfloor = 2;
                    break;
                }
            case FloorOption.Ground:
                {
                    currentfloor = 0;
                    break;
                }
            case FloorOption.Basement:
                {
                    currentfloor = 1;
                    break;
                }
            case FloorOption.Ground2:
                {
                    currentfloor = 9;
                    break;
                }
        }
    }
}
