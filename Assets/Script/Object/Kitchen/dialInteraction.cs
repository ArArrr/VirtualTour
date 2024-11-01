using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class dialInteraction : Interactable
{
    public RotateObject rotateObject;
    public NarrationController narrationController;
    public OvenDial ovenDial;
    public override void Interact3()
    {
        rotateObject.StartRotation();
        ovenDial.TurnOn();
        this.enabled = false;
        EventOnPickup();
    }
}
