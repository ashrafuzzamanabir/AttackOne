using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement; // Required for scene management

public class audioManager : MonoBehaviour
{
    [Header("____________music_____")]
    [SerializeField] public AudioSource musicSource;
    [SerializeField] public AudioSource sfxSource;

    [Header("____________sound_____")]
    public AudioClip background;
    public AudioClip death;
    public AudioClip checkpoint;
    public AudioClip run;
    public AudioClip fire;
    public AudioClip hit;
    public AudioClip enemydeath;
    public AudioClip StartBG;
    public AudioClip EndBG;
    public AudioClip jump;
    public AudioClip win;
    public AudioClip lost;

    private void Start()
    {
        // Get the name of the active scene
        string currentSceneName = SceneManager.GetActiveScene().name;

        // Check if the current scene is the start scene
        if (currentSceneName == "Start") // Replace "StartScene" with the actual name of your start scene
        {
            musicSource.clip = StartBG;
        }
        else if (currentSceneName=="Finish")
        {
            musicSource.clip = EndBG;
        }
        else if (currentSceneName == "control")
        {
            musicSource.clip = EndBG;
        }
        else
        {
            musicSource.clip = background;
        }

        musicSource.loop = true; // Enable looping
        musicSource.Play();      // Play the selected clip
    }

    public void PlaySFX(AudioClip clip)
    {
        sfxSource.PlayOneShot(clip);
    }
}
