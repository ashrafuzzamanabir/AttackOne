using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    // This function loads the next scene
    public void PlayGame()
    {
        SceneManager.LoadScene("dragland");
    }

    public void control()
    {
        SceneManager.LoadScene("control");
    }

    // This function quits the game
    public void QuitGame()
    {
        Debug.Log("Game is exiting...");
        Application.Quit();
    }

    public void back()
    {
        SceneManager.LoadScene("Start");
    }
}
