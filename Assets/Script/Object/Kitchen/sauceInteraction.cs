using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class sauceInteraction : Interactable
{
    // Variables for smooth rotation
    private bool isRotated = false;
    public float rotationSpeed = 2.0f; // Rotation speed, adjust as needed
    private Coroutine rotationCoroutine;
    public Outline outline;
    public override void Interact3()
    {
        if (xrOrigin != null && isAnchored)
        {
            if (rotationCoroutine != null)
            {
                StopCoroutine(rotationCoroutine);
                if (outline != null) outline.enabled = false;
            }

            if (!isRotated)
            {
                rotationCoroutine = StartCoroutine(RotateToAngle(Quaternion.Euler(0, 0, 60f)));
            }
            else
            {
                rotationCoroutine = StartCoroutine(RotateToAngle(originalRotation));
            }

            isRotated = !isRotated;
        }
    }

    private IEnumerator RotateToAngle(Quaternion targetRotation)
    {
        while (Quaternion.Angle(transform.localRotation, targetRotation) > 0.1f)
        {
            transform.localRotation = Quaternion.Lerp(transform.localRotation, targetRotation, rotationSpeed * Time.deltaTime);
            yield return null;
        }
        transform.localRotation = targetRotation;
    }
}
