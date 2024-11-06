using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class fish : MonoBehaviour
{
    public NarrationController nextNarration;
    public AudioClip soundEffect;
    public UnityEvent events;
    private AudioSource source;

    private void Start()
    {
        source = GetComponent<AudioSource>();
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.name == "Cat")
        {
            nextNarration.StartNarration();
            if (soundEffect != null && source != null)
            {
                source.clip = soundEffect;
                source.Play();
            }
            if (events != null) events.Invoke();
        }
    }
}
