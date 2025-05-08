using UnityEngine;

public class Pickup : MonoBehaviour, IInteractable
{
    public string itemName;

    public void Interact()
    {
        // R�cup�re l'inventaire du joueur
        Inventory playerInventory = FindObjectOfType<Inventory>();

        if (playerInventory != null)
        {
            // Ajoute l'objet � l'inventaire
            playerInventory.AddItem(itemName, this.gameObject);

            // D�sactive l'objet au lieu de le d�truire
            gameObject.SetActive(false);

            Debug.Log("Vous avez ramass� : " + itemName);
        }
    }
}
