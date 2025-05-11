using System.Collections.Generic;
using UnityEngine;

public class ShoppingCart : MonoBehaviour, IInteractable
{
    public float dropHeight = 2.0f;
    public float spacing = 0.5f;
    private Vector3 initialDropPosition;

    private void Start()
    {
        // Initialise la position de dépôt
        initialDropPosition = transform.position + Vector3.up * dropHeight;
    }

    public void Interact()
    {
        Inventory inventory = FindObjectOfType<Inventory>();

        if (inventory == null)
        {
            Debug.LogWarning("Aucun inventaire trouvé !");
            return;
        }

        // Réinitialise la position de dépôt
        Vector3 nextDropPosition = initialDropPosition;

        // Récupère tous les objets de l'inventaire
        Dictionary<string, GameObject> items = new Dictionary<string, GameObject>(inventory.GetItems());

        foreach (KeyValuePair<string, GameObject> item in items)
        {
            GameObject itemObject = item.Value;

            // Dépose l'objet sur le caddie
            itemObject.transform.SetParent(this.transform);
            itemObject.transform.position = nextDropPosition;
            itemObject.SetActive(true);

            // Enlève le tag Interactable
            itemObject.tag = "Untagged";

            // Enlève le script Pickup
            Pickup pickupScript = itemObject.GetComponent<Pickup>();
            if (pickupScript != null)
            {
                Destroy(pickupScript);
                Debug.Log("Pickup script removed from " + item.Key);
            }

            // Retire l'objet de l'inventaire
            inventory.DropItem(item.Key);

            // Décale la position pour le prochain objet
            nextDropPosition += Vector3.right * spacing;

            Debug.Log(item.Key + " a été déposé sur le caddie et n'est plus interactif.");
        }

        // Affiche le contenu du caddie
        Debug.Log("Tous les objets ont été déposés sur le caddie.");
    }
}
