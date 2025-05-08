using UnityEngine;

public class Pickup : MonoBehaviour, IInteractable
{
    public string itemName;

    public void Interact()
    {
        // Récupère l'inventaire du joueur
        Inventory playerInventory = FindObjectOfType<Inventory>();

        if (playerInventory != null)
        {
            // Ajoute l'objet à l'inventaire
            playerInventory.AddItem(itemName, this.gameObject);

            // Désactive l'objet au lieu de le détruire
            gameObject.SetActive(false);

            Debug.Log("Vous avez ramassé : " + itemName);
        }
    }
}
