using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class getItem : MonoBehaviour
{
    public List<GameObject> items;

    private MeshRenderer m_Renderer;
    private Outline outline;
    public GameObject ItemToGet;
    public Material transparentMat;
    private Material initialMat;

    [Header("Optional")]
    public GameObject nextItem;
    public Outline outlineItemToGet;

    public GameObject disableObject;

    // Start is called before the first frame update
    void Start()
    {
        foreach (GameObject item in items)
        {
            m_Renderer = item.GetComponent<MeshRenderer>();
            outline = item.GetComponent<Outline>();

            initialMat = m_Renderer.material;
            m_Renderer.material = transparentMat;

            outline.enabled = true;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.name == ItemToGet.name)
        {
            ItemToGet.SetActive(false);
            foreach (GameObject item in items)
            {
                m_Renderer = item.GetComponent<MeshRenderer>();
                outline = item.GetComponent<Outline>();

                outline.enabled = false;
                m_Renderer.material = initialMat;
            }
            if (nextItem != null) nextItem.SetActive(true);
            if (outlineItemToGet != null) outlineItemToGet.enabled = true;
            if (disableObject != null) disableObject.SetActive(false);
        }
    }
}
