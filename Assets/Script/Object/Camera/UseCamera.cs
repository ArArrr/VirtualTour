using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class UseCamera : MonoBehaviour
{
    public InputActionReference mouseClick;

    private void Start()
    {
        mouseClick.action.performed += context => ClickInteraction();
    }

    void ClickInteraction()
    {

        if(this.gameObject.activeSelf == true)
        {
            this.gameObject.SetActive(false);
        }
        else
        {
            this.gameObject.SetActive(true);
        }
        
    }
}
