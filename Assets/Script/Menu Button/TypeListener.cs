using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class TypeListener : MonoBehaviour
{
    public TMP_InputField input;
    public Button btn;

    public void checkText()
    {
        CanvasGroup cg = btn.GetComponent<CanvasGroup>();
        if (input.text == null || input.text == "")
        {
            if (cg != null)
            {
                cg.enabled = true;
            }
        }
        else
        {
            if (cg != null)
            {
                cg.enabled = false;
            }
        }
    }
}
