using UnityEngine;
using TMPro;

public class Pickup : MonoBehaviour, IInteractable
{
    public string itemName;
    public TMP_Text inventoryFullMessage;  // Référence au message UI

    public void Interact()
    {
        // Si l'objet est un Ticket, on le détruit
        if (name == "Ticket")
        {
            Debug.Log("Ticket ramassé et détruit : " + itemName);
            Destroy(gameObject);
            return;
        }

        // Récupère l'inventaire du joueur
        Inventory playerInventory = FindObjectOfType<Inventory>();

        if (playerInventory != null)
        {
            // Vérifie si l'inventaire est plein
            if (playerInventory.IsFull())
            {
                ShowFullInventoryMessage();
                Debug.LogWarning("Inventaire plein, trouvez le caddie !");
                return;
            }

            // Ajoute l'objet à l'inventaire
            playerInventory.AddItem(itemName, this.gameObject);

            // Déplace l'objet sous l'inventaire
            this.transform.SetParent(playerInventory.transform);
            this.transform.localPosition = Vector3.zero;

            // Désactive l'objet
            gameObject.SetActive(false);

            Debug.Log("Vous avez ramassé : " + itemName);
        }
    }

    private void ShowFullInventoryMessage()
    {
        if (inventoryFullMessage != null)
        {
            inventoryFullMessage.text = "Inventaire plein, trouvez le caddie";
            inventoryFullMessage.gameObject.SetActive(true);
            Invoke("HideFullInventoryMessage", 3f);  // Cache le message après 3 secondes
        }
    }

    private void HideFullInventoryMessage()
    {
        if (inventoryFullMessage != null)
        {
            inventoryFullMessage.gameObject.SetActive(false);
        }
    }
}
