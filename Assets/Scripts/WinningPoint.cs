using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;


public class WinningPoint : MonoBehaviour
{
    [SerializeField] private LayerMask playerLayer; // Player layer to check

    public GameManager gameManager;
    public Animator animator;

    [SerializeField] public GameObject winObject; // UI object for winning notification
    [SerializeField] private GameObject creditsCanvas; // Canvas for ending credits
    [SerializeField] private Text creditsText; // Text component to display credits
    [TextArea(5, 10)] public string[] creditsLines; // Array of credit lines
    [SerializeField] private float lineDisplayDuration = 2f; // Time each line is displayed

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

        // Ensure the credits Canvas is disabled at the start
        if (creditsCanvas != null)
        {
            creditsCanvas.SetActive(false);
        }
        else
        {
            Debug.LogWarning("Credits Canvas not assigned in the Inspector.");
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

        if (animator != null)
        {
            animator.SetTrigger("win");
        }
        winObject.SetActive(true);
        Debug.Log($"Collision with: {collision.gameObject.name}");

        // Wait for 3 seconds for win sequence to play
        yield return new WaitForSeconds(3f);

        winObject.SetActive(false); // Hide win message

        // Show and animate the credits Canvas
        if (creditsCanvas != null)
        {
            creditsCanvas.SetActive(true);
            Debug.Log("Displaying line-by-line credits.");

            yield return StartCoroutine(DisplayCreditsLineByLine());

            creditsCanvas.SetActive(false); // Hide the Canvas after credits are done
        }
        else
        {
            Debug.LogWarning("Credits Canvas is missing.");
        }

        // Notify the GameManager to handle the scene transition
        if (gameManager != null)
        {
            gameManager.PlayerWon();
            Debug.Log("Player reached the winning point!");
        }
        else
        {
            Debug.LogError("GameManager reference is missing!");
        }
    }

    private IEnumerator DisplayCreditsLineByLine()
    {
        foreach (string line in creditsLines)
        {
            creditsText.text = line; // Display the current line
            yield return new WaitForSeconds(lineDisplayDuration); // Wait before showing the next line
        }

        // Clear the credits text after all lines are shown
        creditsText.text = "";
    }
}