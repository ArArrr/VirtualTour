using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class cameraInteraction : Interactable
{
    public UseCamera useCamera;
    public override void Start()
    {
        base.Start();
        useCamera = GetComponent<UseCamera>();
    }
    public override void Interact(Vector3 offset)
    {
        base.Interact(offset);
    }
    public override void Interact2()
    {
        
    }
    public override void Interact3()
    {
        if (xrOrigin != null && useCamera != null)
        {
            useCamera.ClickInteraction();
        }
    }
}
