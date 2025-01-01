using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class WinningPoint : MonoBehaviour
{
    [SerializeField] private LayerMask playerLayer; // Player layer to check

    public GameManager gameManager;
    public Animator animator;

    [SerializeField] public GameObject winObject;


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

        if (animator != null)
        {
            animator.SetTrigger("win");
        }
        winObject.SetActive(true);
        Debug.Log($"Collision with: {collision.gameObject.name}");

        // Wait for 4 seconds (or adjust to match the animation/sound duration)
        yield return new WaitForSeconds(3f);

        // Notify the GameManager to handle the scene transition or end game
        if (gameManager != null)
        {
            gameManager.PlayerWon();
            Debug.Log("Player reached the winning point!");
        }
        else
        {
            Debug.LogError("GameManager reference is missing!");
        }
        winObject.SetActive(false);

    }
}
