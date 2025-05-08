using UnityEngine;

// Interface pour les objets interactifs
public interface IInteractable
{
    void Interact();
}

// Script pour détecter les interactions
public class Interactor : MonoBehaviour
{
    public Transform InteractorSource;
    public float InteractRange;

    void Start()
    {
        // Rien à initialiser ici pour l'instant
    }

    void Update()
    {
        // Vérifie si on appuie sur la touche E
        if (Input.GetMouseButtonDown(0))
        {
            // Crée un Ray partant du InteractorSource
            Ray r = new Ray(InteractorSource.position, InteractorSource.forward);

            // Lance le Raycast
            if (Physics.Raycast(r, out RaycastHit hitInfo, InteractRange))
            {
                // Vérifie si l'objet touché a un composant IInteractable
                if (hitInfo.collider.gameObject.TryGetComponent(out IInteractable interactObj))
                {
                    // Lance l'interaction
                    interactObj.Interact();
                }
            }
        }
    }
}
