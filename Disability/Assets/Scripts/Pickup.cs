using UnityEngine;
using TMPro;

public class Pickup : MonoBehaviour, IInteractable
{
    public string itemName;
    public TMP_Text inventoryFullMessage;  // R�f�rence au message UI

    public void Interact()
    {
        // R�cup�re l'inventaire du joueur
        Inventory playerInventory = FindObjectOfType<Inventory>();

        if (playerInventory != null)
        {
            // V�rifie si l'inventaire est plein
            if (playerInventory.IsFull())
            {
                ShowFullInventoryMessage();
                Debug.LogWarning("Inventaire plein, trouvez le caddie !");
                return;
            }

            // Ajoute l'objet � l'inventaire
            playerInventory.AddItem(itemName, this.gameObject);

            // D�place l'objet sous l'inventaire
            this.transform.SetParent(playerInventory.transform);
            this.transform.localPosition = Vector3.zero;

            // D�sactive l'objet
            gameObject.SetActive(false);

            Debug.Log("Vous avez ramass� : " + itemName);
        }
    }

    private void ShowFullInventoryMessage()
    {
        if (inventoryFullMessage != null)
        {
            inventoryFullMessage.text = "Inventaire plein, trouvez le caddie";
            inventoryFullMessage.gameObject.SetActive(true);
            Invoke("HideFullInventoryMessage", 3f);  // Cache le message apr�s 3 secondes
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
