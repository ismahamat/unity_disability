using System.Collections.Generic;
using UnityEngine;

public class ShoppingCart : MonoBehaviour, IInteractable
{
    public float dropHeight = 2.0f;
    public float spacing = 0.5f;
    private Vector3 initialDropPosition;

    private void Start()
    {
        // Initialise la position de d�p�t
        initialDropPosition = transform.position + Vector3.up * dropHeight;
    }

    public void Interact()
    {
        Inventory inventory = FindObjectOfType<Inventory>();

        if (inventory == null)
        {
            Debug.LogWarning("Aucun inventaire trouv� !");
            return;
        }

        // R�initialise la position de d�p�t
        Vector3 nextDropPosition = initialDropPosition;

        // R�cup�re tous les objets de l'inventaire
        Dictionary<string, GameObject> items = new Dictionary<string, GameObject>(inventory.GetItems());

        foreach (KeyValuePair<string, GameObject> item in items)
        {
            GameObject itemObject = item.Value;

            // D�pose l'objet sur le caddie
            itemObject.transform.SetParent(this.transform);
            itemObject.transform.position = nextDropPosition;
            itemObject.SetActive(true);

            // Enl�ve le tag Interactable
            itemObject.tag = "Untagged";

            // Enl�ve le script Pickup
            Pickup pickupScript = itemObject.GetComponent<Pickup>();
            if (pickupScript != null)
            {
                Destroy(pickupScript);
                Debug.Log("Pickup script removed from " + item.Key);
            }

            // Retire l'objet de l'inventaire
            inventory.DropItem(item.Key);

            // D�cale la position pour le prochain objet
            nextDropPosition += Vector3.right * spacing;

            Debug.Log(item.Key + " a �t� d�pos� sur le caddie et n'est plus interactif.");
        }

        // Affiche le contenu du caddie
        Debug.Log("Tous les objets ont �t� d�pos�s sur le caddie.");
    }
}
