using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayInstance : MonoBehaviour
{
    public void saveInfo()
    {
        DataManager.Instance.GetComponent<ProgressManager>().saveInfo();
    }

    public void startTimer()
    {
        if (DataManager.Instance.isDemo)
        {
            LevelManager.Instance.GetComponentInChildren<DemoModeCountdown>().StartCountdown();
            Debug.Log("Starting Countdown");
        }
    }
}
