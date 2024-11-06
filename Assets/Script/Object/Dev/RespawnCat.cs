using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class RespawnCat : MonoBehaviour
{
    public GameObject cat;
    public UnityEvent events;
    private void OnTriggerEnter(Collider other)
    {
        if (other.name == "Cat")
        {
            events.Invoke();
        }
    }
    public void returnPosition()
    {
        cat.transform.localPosition = new Vector3(-331f, -150f, 0f);
    }
}
