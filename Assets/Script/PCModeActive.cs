using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine.XR.Interaction.Toolkit.Filtering;

public class PCModeActive : MonoBehaviour
{
    public List<GameObject> gameObjects;
    public List<XRSimpleInteractable> interactables;
    public List<XRPokeFilter> prefilters;

    private void Start()
    {
        StartCoroutine(WaitForUpdate());
    }

    private IEnumerator WaitForUpdate()
    {
        int check = 0;
        while (!DataManager.Instance.togglePC)
        {
            yield return new WaitForSeconds(1f); // Check every 1 seconds to reduce overhead
            check++;
            if (check == 10)
            {
                Destroy(this);
            }
        }
        foreach (GameObject go in gameObjects)
        {
            if (go != null)
            {
                go.SetActive(true);
            }
        }
        foreach (XRSimpleInteractable inter in interactables)
        {
            if (inter != null)
            {
                inter.enabled = false;
            }
        }
        foreach (XRPokeFilter prefil in prefilters)
        {
            if (prefil != null)
            {
                prefil.enabled = false;
            }
        }
        yield break;
    }
}
