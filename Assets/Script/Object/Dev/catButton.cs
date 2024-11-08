using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class catButton : MonoBehaviour
{
    public Button btn;
    public UnityEvent events;
    public NarrationController controller;
    public void Invoke()
    {
        if (events != null && btn.enabled)
        {
            events.Invoke();
            if (controller.count == controller.currentCount )
            {
                controller.StartNarration();
            }
        }
    }
}
