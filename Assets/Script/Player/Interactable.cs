using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Interactable : MonoBehaviour
{
    public string message;
    private Outline outline;
    private Transform xrOrigin;
    private Rigidbody rb;
    private bool isAnchored = false; // Track if the object is currently anchored
    private Transform originalParent; // Store the original parent

    private void Start()
    {
        outline = GetComponent<Outline>();
        if (outline != null)
        {
            outline.enabled = false;
        }

        // Locate the "XR Origin (VR)" object in the scene
        GameObject xrOriginObj = GameObject.Find("XR Origin (VR)");
        if (xrOriginObj != null)
        {
            xrOrigin = xrOriginObj.transform;
        }
        else
        {
            Debug.LogError("XR Origin (VR) object not found in the scene.");
        }

        // Automatically get the Rigidbody component and set it as non-kinematic by default
        rb = GetComponent<Rigidbody>();
        //if (rb != null)
        //{
        //    rb.isKinematic = false;
        //}

        // Save the original parent transform
        originalParent = transform.parent;
    }

    public void Interact(Vector3 offset)
    {
        if (xrOrigin != null)
        {
            if (!isAnchored)
            {
                // Anchor the object to XR Origin
                transform.SetParent(xrOrigin);
                transform.localPosition = offset;

                // Set Rigidbody to kinematic to prevent physics interactions
                if (rb != null)
                {
                    rb.isKinematic = true;
                }

                Debug.Log("Object anchored to XR Origin with offset.");
            }
            else
            {
                // Unanchor the object and set it back to its original parent
                transform.SetParent(originalParent);

                // Reset position if it's tagged as "IDCard"
                if (CompareTag("IDCard"))
                {
                    transform.localPosition = Vector3.zero;
                }

                // Set Rigidbody back to non-kinematic for regular physics interactions
                if (rb != null)
                {
                    rb.isKinematic = false;
                }

                Debug.Log("Object unanchored and returned to original parent.");
            }
            // Toggle the anchored state
            isAnchored = !isAnchored;
        }
    }

    public void DisableOutline()
    {
        if (outline != null)
        {
            outline.enabled = false;
        }
    }

    public void EnableOutline()
    {
        if (outline != null)
        {
            outline.enabled = true;
        }
    }
}
