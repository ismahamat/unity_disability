using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    public void PlayGame()
    {
        // Charger la première scène du jeu (index 1)
        SceneManager.LoadScene(1);
    }

}
