using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class DoorControllerBasedOnMovement : MonoBehaviour
{
    public GameObject doorHandle;
    public DistanceReleaseGrabInteractable grabInteractable;
    public float movementThreshold = 0.25f;
    
    private Animator _doorAnimator;
    private Vector3 _startPosition;
    private bool _handleGrabbed = false;
    private bool _doorOpened = false;

    private void Awake()
    {
        if(!doorHandle)
        {
            Debug.LogError("Door handle not assigned in the inspector.");
            enabled = false;
            return;
        }
        _doorAnimator = GetComponent<Animator>();
        if (!_doorAnimator)
        {
            Debug.LogError("Door Controller missing Animator.");
            enabled = false;
            return;
        }
        if (!grabInteractable)
        {
            Debug.LogError("Door Controller missing grabInteractable in the inspector.");
            enabled = false;
            return;
        }
        grabInteractable.selectEntered.AddListener(HandleGrabbed);
        grabInteractable.selectExited.AddListener(HandleReleased);
    }
    private void OnDestroy()
    {
        if (grabInteractable)
        {
            grabInteractable.selectEntered.RemoveListener(HandleGrabbed);
            grabInteractable.selectExited.RemoveListener(HandleReleased);
        }
    }
    private void HandleGrabbed(SelectEnterEventArgs args)
    {
        _startPosition = doorHandle.transform.position;
        _handleGrabbed = true;
    }
    private void HandleReleased(SelectExitEventArgs args)
    {
        _handleGrabbed = false;
    }

    private void Update()
    {
        if (_handleGrabbed)
        {
            float distanceMoved = Vector3.Distance(doorHandle.transform.position, _startPosition);
            if (distanceMoved > movementThreshold && !_doorOpened)
            {
                grabInteractable.DetachInteractor();
                Debug.Log("isOpen: " + _doorOpened);
                _doorAnimator.SetBool("isOpen", true);
                _doorOpened = true;
            }
            else if (distanceMoved > movementThreshold && _doorOpened)
            {
                grabInteractable.DetachInteractor();
                Debug.Log("isOpen: " + _doorOpened);
                _doorAnimator.SetBool("isOpen", false);
                _doorOpened = false;
            }
        }
    }
}
