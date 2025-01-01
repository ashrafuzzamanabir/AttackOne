using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class nextlevel : MonoBehaviour
{
    [SerializeField] private LayerMask playerLayer; // Player layer to check
    private GameManager gameManager;
    //public Animator animator;
    [SerializeField] private Text levelText;
    audioManager audioManager;

    private void Awake()
    {
        audioManager = GameObject.FindGameObjectWithTag("Audio").GetComponent<audioManager>();
    }

    private void Start()
    {
        gameManager = FindObjectOfType<GameManager>();
        if (gameManager == null)
        {
            Debug.LogError("GameManager not found in the scene!");
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        // Check if the collision object is from the Player Layer
        if (((1 << collision.gameObject.layer) & playerLayer) != 0)
        {
            StartCoroutine(HandleWinSequence(collision));

        }
    }
    private IEnumerator HandleWinSequence(Collider2D collision)
    {
        // Play the win sound and trigger the animation
        if (audioManager != null)
        {
            audioManager.PlaySFX(audioManager.win);
        }

        //if (animator != null)
        //{
        //    animator.SetTrigger("win");
        //}

        if (levelText != null)
        {
            levelText.text = "Level Complete! Loading Next Level..."; // Update the text
            levelText.gameObject.SetActive(true); // Show the text
        }

        Debug.Log($"Collision with: {collision.gameObject.name}");

        // Wait for 2 seconds to allow the player to see the text
        yield return new WaitForSeconds(2f);


        // Load the finish scene
        SceneManager.LoadScene("Level2");
    }


}
