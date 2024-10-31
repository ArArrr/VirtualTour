using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class ReturnToMenu : MonoBehaviour
{
    public InputActionProperty showButton;

    // Update is called once per frame
    void Update()
    {
        if (showButton.action.WasPressedThisFrame())
        {
            LevelManager.Instance.LoadScene("Main Menu", "CrossFade", "none");
        }
    }
}
