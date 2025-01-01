using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class floatMenu : MonoBehaviour
{

    [SerializeField] GameObject playButton;
    [SerializeField] GameObject pauseButton;
    [SerializeField] GameObject quitButton;


    public void PlayGame()
    {
        Time.timeScale = 1;
        playButton.SetActive(false);
        pauseButton.SetActive(true);
        quitButton.SetActive(false);

    }

    // Update is called once per frame
    public void PauseGame()
    {
        Time.timeScale = 0;
        playButton.SetActive(true);
        pauseButton.SetActive(false);
        quitButton.SetActive(true);
    }

    public void QuitGame()
    {
        Debug.Log("Game is exiting...");
        Application.Quit();
    }

}
