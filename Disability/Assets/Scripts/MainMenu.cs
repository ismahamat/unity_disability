using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    public void PlayGame()
    {
        // Charger la premi�re sc�ne du jeu (index 1)
        SceneManager.LoadScene(1);
    }

}
