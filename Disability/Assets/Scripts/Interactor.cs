using UnityEngine;

public interface IInteractable
{
    void Interact();
}

public class Interactor : MonoBehaviour
{
    public Transform InteractorSource;
    public float InteractRange;

    void Start()
    {
    }

    void Update()
    {
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
