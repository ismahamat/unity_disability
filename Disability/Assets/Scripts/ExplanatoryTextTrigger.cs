using UnityEngine;
using TMPro;

public class ExplanatoryTextTrigger : MonoBehaviour
{
    public GameObject explanatoryPanel;


    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            explanatoryPanel.SetActive(true);

        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            explanatoryPanel.SetActive(false);

        }
    }
}
