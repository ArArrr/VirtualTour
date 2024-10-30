using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Interactable : MonoBehaviour
{
    public string tipOnHover = "Press [E] to pick up";
    public string tipOnEquip = "[E] Drop [G] Throw";
    public float throwStrength = 10f;
    protected Outline outline;
    protected Transform xrOrigin;
    protected Rigidbody rb;
    public bool isAnchored = false; // Track if the object is currently anchored
    public Transform originalParent; // Store the original parent
    protected Quaternion originalRotation;
    protected bool isKinematic;
    public Quaternion rotation;
    public bool doNotRotate;
    public bool keepOldRotation;
    public Vector3 positionOffset;

    // Action event for interactions
    public Action<Vector3> OnInteract;
    public Action Dropped;

    [Header("Event on Pickup")]
    public UnityEvent unityEvent;
    public int pickupCount = 0;
    public bool onlyOnce = false;

    public virtual void Start()
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
        if (rb != null)
        {
            if (rb.isKinematic)
            {
                isKinematic = true;
            }
        }

        // Save the original parent transform
        originalParent = transform.parent;
        if (keepOldRotation)
        {
            originalRotation = transform.localRotation;
        }
    }

    public void Interact(Vector3 offset)
    {
        if (xrOrigin != null)
        {
            if (!isAnchored)
            {
                // Anchor the object to XR Origin
                transform.SetParent(xrOrigin);
                transform.localPosition = offset+positionOffset;
                if (!doNotRotate)
                transform.localRotation = rotation;

                // Set Rigidbody to kinematic to prevent physics interactions
                if (rb != null)
                {
                    rb.isKinematic = true;
                }
                isAnchored = true;
                EventOnPickup();
                Debug.Log("Object anchored to XR Origin with offset.");
            }
            else
            {
                // Unanchor the object and set it back to its original parent
                if (transform.parent != originalParent) transform.SetParent(originalParent);
                else transform.SetParent(null);

                // Reset position if it's tagged as "IDCard"
                if (CompareTag("IDCard"))
                {
                    transform.localPosition = Vector3.zero;
                }
                if (keepOldRotation)
                {
                    transform.localRotation = originalRotation;
                }

                // Set Rigidbody back to non-kinematic for regular physics interactions
                if (rb != null && !isKinematic)
                {
                    rb.isKinematic = false;
                }
                isAnchored = false;
                Debug.Log("Object unanchored and returned to original parent.");
                Dropped?.Invoke();
            }
            // Toggle the anchored state
        } else
        {
            Debug.LogError("XR Origin (VR) object not found in the scene.");
        }
        OnInteract?.Invoke(offset);
    }

    public virtual void Interact2()
    {
        if (xrOrigin != null && isAnchored && rb != null && !isKinematic)
        {
            if (CompareTag("IDCard")) return;
            // Unanchor the object and set it back to its original parent
            if (transform.parent != originalParent) transform.SetParent(originalParent);
            else transform.SetParent(null);

            // Set Rigidbody back to non-kinematic for regular physics interactions
            rb.isKinematic = false;

            // Calculate throw direction from the camera's forward vector
            Camera mainCamera = Camera.main;
            if (mainCamera != null)
            {
                Vector3 throwDirection = mainCamera.transform.forward;

                // Apply force to throw the object
                rb.AddForce(throwDirection * throwStrength, ForceMode.VelocityChange);

                Debug.Log("Object thrown in the camera's forward direction.");
            }
            else
            {
                Debug.LogError("Main Camera not found for throw direction.");
            }
            DataManager.Instance.isHoldingItem = false;
            isAnchored = false;
            
            // Invoke Dropped action to notify other scripts if needed
            Dropped?.Invoke();
            // Toggle anchored state
        }
        else
        {
            Debug.LogWarning("XR Origin (VR) or Rigidbody component not found, or object is not anchored.");
        }
    }

    public virtual void Interact3()
    {
        if (xrOrigin != null)
        {
            if (isAnchored)
            {

            }
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

    public void setCameraInUse(bool val)
    {
        DataManager.Instance.cameraInUse = val;
    }
    public void OnDisable()
    {
        
    }
    public void EventOnPickup()
    {
        if(unityEvent != null)
        {
            if (onlyOnce && pickupCount > 0) return;
            unityEvent.Invoke();
        }
        pickupCount++;
    }
}
