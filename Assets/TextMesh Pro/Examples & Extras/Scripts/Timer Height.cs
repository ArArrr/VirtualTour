using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class TimerHeight : MonoBehaviour
{
    public TMP_InputField inputField;
    private void Start()
    {
        Transform timerTransform = LevelManager.Instance.transform.Find("Timer");
        if (timerTransform != null)
        {
            // Modify the x-axis position
            Vector3 newPosition = timerTransform.localPosition;
            inputField.text = newPosition.x.ToString();
        }
    }
    public void changeHeight()
    {
        Transform timerTransform = LevelManager.Instance.transform.Find("Timer");
        if (timerTransform != null)
        {
            Vector3 newPosition = timerTransform.localPosition;
            newPosition.x = int.Parse(inputField.text);
            timerTransform.localPosition = newPosition;
        }
    }
}
