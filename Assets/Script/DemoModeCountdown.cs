using System.Collections;
using UnityEngine;
using TMPro;

public class DemoModeCountdown : MonoBehaviour
{
    [Tooltip("Reference to the TextMeshPro component for displaying the countdown.")]
    public TextMeshProUGUI countdownText;

    [Tooltip("Total time for the countdown in seconds.")]
    public float countdownTime = 300;
    private bool countdownActive = false;

    private void Start()
    {
        countdownText.enabled = false;
    }

    // Starts the countdown timer
    public void StartCountdown()
    {
        countdownTime = DataManager.Instance.timer * 60f;
        countdownActive = true;
        countdownText.enabled = true;
        StartCoroutine(CountdownRoutine());
    }

    private IEnumerator CountdownRoutine()
    {
        float remainingTime = countdownTime;

        while (remainingTime > 0)
        {
            // Convert remaining time to minutes and seconds using integer division
            int minutes = Mathf.FloorToInt(remainingTime / 60);
            int seconds = Mathf.FloorToInt(remainingTime % 60);

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
        countdownText.enabled = false;
        countdownActive = false;
    }
}
