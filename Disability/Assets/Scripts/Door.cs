using UnityEngine;

public class Door : MonoBehaviour, IInteractable
{
    public bool isOpen = false;
    public float openAngle = 90f;
    public float closeAngle = 0f;
    public float speed = 3f;

    private Quaternion targetRotation;

    void Start()
    {
        targetRotation = Quaternion.Euler(0, closeAngle, 0);
    }

    void Update()
    {
        // Smooth rotation vers la cible
        transform.localRotation = Quaternion.Slerp(transform.localRotation, targetRotation, Time.deltaTime * speed);
    }

    public void Interact()
    {
        // Change l'état de la porte
        isOpen = !isOpen;
        targetRotation = Quaternion.Euler(0, isOpen ? openAngle : closeAngle, 0);
        Debug.Log(gameObject.name + " s'est " + (isOpen ? "ouverte" : "fermée"));
    }
}
