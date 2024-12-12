using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class TimerHeight : MonoBehaviour
{
    public TMP_InputField inputField;
    private void Start()
    {
        Transform timerTransform = GameObject.Find("Time Counter").transform;
        if (timerTransform != null)
        {
            // Modify the x-axis position
            Vector3 newPosition = timerTransform.localPosition;
            inputField.text = newPosition.y.ToString();
        }

        if (DataManager.Instance.togglePC == false)
        {
            Vector3 newPosition = timerTransform.localPosition;
            newPosition.y = 216;
            timerTransform.localPosition = newPosition;
        } else
        {
            Vector3 newPosition = timerTransform.localPosition;
            newPosition.y = 268;
            timerTransform.localPosition = newPosition;
        }
    }
    public void changeHeight()
    {
        Transform timerTransform = GameObject.Find("Time Counter").transform;
        if (timerTransform != null)
        {
            Vector3 newPosition = timerTransform.localPosition;
            newPosition.y = int.Parse(inputField.text);
            timerTransform.localPosition = newPosition;
        }
    }
}
