using UnityEngine;

public class HighlightInteractable : MonoBehaviour
{
    public LayerMask interactableLayer;
    private GameObject currentObject;
    public float highlightDistance = 3f;

    void Update()
    {
        Ray ray = new Ray(transform.position, transform.forward);
        RaycastHit hit;

        // Lance le Raycast avec Layer Mask
        if (Physics.Raycast(ray, out hit, highlightDistance, interactableLayer))
        {
            // Vérifie si l'objet a un Tag "Interactable"
            if (hit.transform.CompareTag("Interactable"))
            {
                GameObject hitObject = hit.transform.gameObject;

                // Si on vise un nouvel objet
                if (currentObject != hitObject)
                {
                    ResetOutline();
                    currentObject = hitObject;

                    // Active l'Outline
                    Outline outline = currentObject.GetComponent<Outline>();
                    if (outline != null)
                    {
                        outline.enabled = true;
                    }
                }
            }
        }
        else
        {
            ResetOutline();
        }
    }

    void ResetOutline()
    {
        if (currentObject != null)
        {
            Outline outline = currentObject.GetComponent<Outline>();
            if (outline != null)
            {
                outline.enabled = false;
            }

            currentObject = null;
        }
    }
}
