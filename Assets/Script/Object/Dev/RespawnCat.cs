using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class RespawnCat : MonoBehaviour
{
    public GameObject cat;
    public UnityEvent events;
    public bool triggered = false;
    private void OnTriggerEnter(Collider other)
    {
        if (other.name == "Cat")
        {
            if (!triggered)
            events.Invoke();
            triggered = true;
            Debug.Log(gameObject.name+" Triggered");
        }
    }
    public void returnPosition()
    {
        cat.transform.localPosition = new Vector3(-331f, -150f, 0f);
    }
    public void resetTrigger()
    {
        triggered = false;
    }
}
