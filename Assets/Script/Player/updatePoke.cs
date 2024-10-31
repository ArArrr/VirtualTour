using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class updatePoke : MonoBehaviour
{
    XRPokeInteractor LeftPoke;
    XRPokeInteractor RightPoke;
    public Transform newLeft;
    public Transform newRight;

    private void Start()
    {
        // Find Left and Right Poke Interactors
        GameObject left = GameObject.Find("Left Poke Interactor");
        GameObject right = GameObject.Find("Right Poke Interactor");

        if (left != null) LeftPoke = left.GetComponent<XRPokeInteractor>();
        if (right != null) RightPoke = right.GetComponent<XRPokeInteractor>();

        // Find the nested `f_index.03.L_end` and `f_index.03.R_end` transforms
        newLeft = FindChildTransformByName(transform, "f_index.03.L_end");
        newRight = FindChildTransformByName(transform, "f_index.03.R_end");

        // Assign to Left and Right Poke Interactors
        if (LeftPoke != null && newLeft != null) LeftPoke.attachTransform = newLeft;
        if (RightPoke != null && newRight != null) RightPoke.attachTransform = newRight;
    }

    // Recursive function to search for a transform by name
    private Transform FindChildTransformByName(Transform parent, string name)
    {
        foreach (Transform child in parent)
        {
            if (child.name == name)
                return child;

            Transform found = FindChildTransformByName(child, name);
            if (found != null)
                return found;
        }
        return null;
    }
}
