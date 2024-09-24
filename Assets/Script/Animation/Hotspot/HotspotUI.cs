using UnityEngine;

public class HotspotUI : MonoBehaviour
{
    public Animator infoUIAnimator; // Reference to the Animator controlling the "Info UI"
    public string closeTrigger = "Close"; // Animator trigger names
    public string farTrigger = "Far";
    public string openTrigger = "Open";

    private void OnTriggerEnter(Collider other)
    {
        // Assuming the player has the tag "Player" or "MainCamera"
        if (other.CompareTag("Player") || other.CompareTag("MainCamera"))
        {
            // Trigger "Close" animation when the player enters the sphere collider
            infoUIAnimator.SetTrigger(closeTrigger);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player") || other.CompareTag("MainCamera"))
        {
            // Trigger "Far" animation when the player exits the sphere collider
            infoUIAnimator.SetTrigger(farTrigger);
        }
    }

    // Method to be used for On Click() button event to trigger "Close" animation
    public void CloseUI()
    {
        infoUIAnimator.SetTrigger(closeTrigger);
    }

    // Method to be used for On Click() button event to trigger "Open" animation
    public void OpenUI()
    {
        infoUIAnimator.SetTrigger(openTrigger);
    }
}
