using System.Collections.Generic;
using UnityEngine;

public class Inventory : MonoBehaviour
{
    private Dictionary<string, GameObject> items = new Dictionary<string, GameObject>();

    public void AddItem(string itemName, GameObject itemObject)
    {
        if (!items.ContainsKey(itemName))
        {
            items.Add(itemName, itemObject);
            Debug.Log("Vous avez ajouté " + itemName + " à l'inventaire.");
        }
        else
        {
            Debug.Log(itemName + " est déjà dans l'inventaire.");
        }
    }

    public void ShowInventory()
    {
        Debug.Log("Inventaire : " + string.Join(", ", items.Keys));
    }

    public bool HasItem(string itemName)
    {
        return items.ContainsKey(itemName);
    }

    public void DropItem(string itemName)
    {
        if (items.ContainsKey(itemName))
        {
            GameObject itemObject = items[itemName];
            itemObject.SetActive(true);
            items.Remove(itemName);
            Debug.Log("Vous avez lâché : " + itemName);
        }
        else
        {
            Debug.Log(itemName + " n'est pas dans l'inventaire.");
        }
    }
}
