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
        LevelManager.Instance.GetComponentInChildren<DemoModeCountdown>().StartCountdown();
    }
}
