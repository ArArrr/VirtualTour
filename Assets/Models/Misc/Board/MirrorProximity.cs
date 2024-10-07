using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MirrorProximity : MonoBehaviour
{
    [SerializeField] GameObject Camera;
    // Start is called before the first frame update
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            Camera.SetActive(true);
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            Camera.SetActive(false);
        }
    }
}
