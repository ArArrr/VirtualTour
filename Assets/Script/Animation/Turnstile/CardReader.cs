using UnityEngine;

public class CardReaderTrigger : MonoBehaviour
{
    public TurnstileController turnstileController; // Reference to the parent Turnstile's main script
    public bool isEntryReader = false;              // Specify if it's "IN" (true) or "OUT" (false)

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("IDCard"))
        {
            // Notify the parent Turnstile controller about the detected card
            turnstileController.OnCardRead(isEntryReader);
        }
    }
}
