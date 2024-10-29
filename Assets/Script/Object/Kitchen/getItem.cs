using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class getItem : MonoBehaviour
{
    public List<GameObject> items;
    private Dictionary<GameObject, Material> initialMaterials = new Dictionary<GameObject, Material>();
    private MeshRenderer m_Renderer;
    private Outline outline;
    public GameObject ItemToGet;
    public Material transparentMat;

    [Header("Optional")]
    public GameObject nextItem;
    public Outline outlineItemToGet;
    public GameObject activateObject;
    public GameObject disableObject;

    [Header("Next Narration")]
    public NarrationController nextNarration;  // Reference to the next narration controller

    // Start is called before the first frame update
    void Start()
    {
        foreach (GameObject item in items)
        {
            m_Renderer = item.GetComponent<MeshRenderer>();
            outline = item.GetComponent<Outline>();
            initialMaterials[item] = m_Renderer.material;
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
                m_Renderer.material = initialMaterials[item];
            }
            if (nextNarration != null) nextNarration.StartNarration();
            if (nextItem != null) nextItem.SetActive(true);
            if (outlineItemToGet != null) outlineItemToGet.enabled = true;
            if (disableObject != null) disableObject.SetActive(false);
            if (activateObject != null) activateObject.SetActive(true);
            
        }
    }
}
