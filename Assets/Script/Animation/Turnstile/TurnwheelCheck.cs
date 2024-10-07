using UnityEngine;

public class WheelTrigger : MonoBehaviour
{
    public TurnstileController turnstileController; // Reference to the parent Turnstile's main script

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            turnstileController.OnPlayerEnteredWheel();
        }
    }
}
