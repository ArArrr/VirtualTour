using System.Collections;
using System.Collections.Generic;
using Unity.XR.CoreUtils;
using UnityEngine;

public class dough2Interactable : Interactable
{
    NextDough nextDough;
    public GameObject dough2;
    public float tossHeight = 1f;  // Height of the toss in the Y-axis
    public float tossDuration = 1f; // Duration to return to the original position

    private Vector3 originalPosition;
    private bool isTossing = false;

    public override void Start()
    {
        base.Start();
        nextDough = dough2.GetComponent<NextDough>();
    }
    public override void Interact3()
    {
        if (xrOrigin != null && isAnchored && rb != null && !isKinematic)
        {
            originalPosition = transform.localPosition;
            Toss();
        }
    }
    public void Toss()
    {
        if (!isTossing)
        {
            StartCoroutine(TossAndReturn());
        }
    }
    private IEnumerator TossAndReturn()
    {
        isTossing = true;

        // Target position for the toss
        Vector3 tossPosition = originalPosition + new Vector3(0, tossHeight, 0);
        nextDough.isInAir = true;
        nextDough.isSpinning = true;
        nextDough.SpinDough();
        // Move the object up to the toss position
        float elapsedTime = 0f;
        while (elapsedTime < tossDuration / 2)
        {
            transform.localPosition = Vector3.Lerp(originalPosition, tossPosition, elapsedTime / (tossDuration / 2));
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        // Ensure it reaches the exact toss position
        transform.localPosition = tossPosition;

        // Return the object to the original position
        elapsedTime = 0f;
        while (elapsedTime < tossDuration / 2)
        {
            transform.localPosition = Vector3.Lerp(tossPosition, originalPosition, elapsedTime / (tossDuration / 2));
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        // Ensure it reaches the exact original position
        transform.localPosition = originalPosition;

        nextDough.isInAir = false;
        nextDough.isSpinning = false;

        isTossing = false;
    }
}
