using UnityEngine;

public class InteractableObject : MonoBehaviour
{
    public Material highlightMaterial;
    private Material[] originalMaterials;
    private Renderer rend;
    private bool isHighlighted = false;

    void Awake()
    {
        rend = GetComponent<Renderer>();
        if (rend != null)
        {
            originalMaterials = rend.materials;
        }
    }

    public void Highlight()
    {
        if (isHighlighted || rend == null || highlightMaterial == null)
            return;

        Material[] highlightMats = new Material[rend.materials.Length];
        for (int i = 0; i < highlightMats.Length; i++)
            highlightMats[i] = highlightMaterial;

        rend.materials = highlightMats;
        isHighlighted = true;
    }

    public void RemoveHighlight()
    {
        if (!isHighlighted || rend == null || originalMaterials == null)
            return;

        rend.materials = originalMaterials;
        isHighlighted = false;
    }

    public void PickUp()
    {
        RemoveHighlight();
        gameObject.SetActive(false);
    }
}
