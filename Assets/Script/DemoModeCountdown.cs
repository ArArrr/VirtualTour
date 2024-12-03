using System.Collections;
using UnityEngine;
using TMPro;

public class DemoModeCountdown : MonoBehaviour
{
    [Tooltip("Reference to the TextMeshPro component for displaying the countdown.")]
    public TextMeshProUGUI countdownText;

    [Tooltip("Total time for the countdown in seconds.")]
    public float countdownTime = DataManager.Instance.timer * 60f;
    private bool countdownActive = false;

    private void Start()
    {
        if (DataManager.Instance.isDemo)
        {
            StartCountdown();
        } 
        else
        {
            gameObject.SetActive(false);
        }
        
    }

    // Starts the countdown timer
    public void StartCountdown()
    {
        countdownActive = true;
        StartCoroutine(CountdownRoutine());
    }

    private IEnumerator CountdownRoutine()
    {
        float remainingTime = countdownTime;

        while (remainingTime > 0)
        {
            // Convert remaining time to minutes and seconds
            float minutes = remainingTime / 60;
            float seconds = remainingTime % 60;

            // Update the TextMeshPro component
            countdownText.text = $"{minutes:00} : {seconds:00}";

            // Wait for one second
            yield return new WaitForSeconds(1);

            remainingTime--;
        }

        // When the countdown ends, show "00 : 00" and teleport the player
        countdownText.text = "00 : 00";
        TeleportPlayerToEnd();
    }

    // Teleports the player to the end of the game
    private void TeleportPlayerToEnd()
    {
        LevelManager.Instance.LoadScene("Outdoor", "CrossFade", "none");
        gameObject.SetActive(false);
    }
}
