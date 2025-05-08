using UnityEngine;
using System.Collections.Generic;

public class PlayerItemDetector : MonoBehaviour
{
    public Camera playerCamera;
    public float detectionRange = 3f;

    private InteractableObject currentObject;
    private List<string> inventory = new List<string>();

    void Update()
    {
        InteractableObject detected = null;

        Ray ray = new Ray(playerCamera.transform.position, playerCamera.transform.forward);
        if (Physics.Raycast(ray, out RaycastHit hit, detectionRange))
        {
            detected = hit.collider.GetComponent<InteractableObject>();
        }

        // ➕ Highlight si on vise un nouveau
        if (detected != currentObject)
        {
            if (currentObject != null)
                currentObject.RemoveHighlight();

            if (detected != null)
                detected.Highlight();

            currentObject = detected;
        }

        // 🧲 Ramassage
        if (Input.GetKeyDown(KeyCode.E) && currentObject != null)
        {
            inventory.Add(currentObject.name);
            Debug.Log("Ramassé : " + currentObject.name);
            currentObject.PickUp();
            currentObject = null;
        }

        // 👁‍🗨 Inventaire console
        if (Input.GetKeyDown(KeyCode.I))
        {
            Debug.Log("Inventaire :");
            foreach (string item in inventory)
                Debug.Log("- " + item);
        }
    }
}
