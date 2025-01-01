using System.Collections; 
using System.Collections.Generic;
using UnityEngine;


public class Enemy : MonoBehaviour
{
    public int health = 100; // Enemy health
    [SerializeField] private LayerMask playerLayer; // Layer for the player
    private GameManager gameManager; // Reference to the GameManager

    audioManager audioManager;
    private void Awake()
    {
        audioManager = GameObject.FindGameObjectWithTag("Audio").GetComponent<audioManager>();
    }


    private void Start()
    {
        gameManager = FindObjectOfType<GameManager>(); // Get GameManager from the scene
        if (gameManager == null)
        {
            Debug.LogError("GameManager not found in the scene!");
        }
    }

    /// <summary>
    /// Reduces the enemy's health by the given damage and checks for death.
    /// </summary>
    /// <param name="damage">Amount of damage to apply.</param>
    public void TakeDamage(int damage)
    {
        health -= damage; // Reduce health
        Debug.Log($"Enemy health: {health}");
        if (health <= 0)
        {
            audioManager.PlaySFX(audioManager.enemydeath);
            Die();
        }
    }

    /// <summary>
    /// Handles enemy death.
    /// </summary>
    private void Die()
    {
        Destroy(gameObject); // Destroy the enemy object
        Debug.Log("Enemy destroyed!");
    }

    /// <summary>
    /// Handles collisions with the player and triggers respawn.
    /// </summary>
    /// <param name="collision">The collision object.</param>
    private void OnCollisionEnter2D(Collision2D collision)
    {
        // Check if the collision object is on the player layer
        if (((1 << collision.gameObject.layer) & playerLayer) != 0)
        {
            PlayerController player = collision.gameObject.GetComponent<PlayerController>();

            if (player != null)
            {
                player.Die(); // Trigger player's death animation

                //if (gameManager != null)
                //{
                //    gameManager.PlayerDied(); // Notify GameManager about the player's death
                //}
                //else
                //{
                //    Debug.LogError("GameManager reference is missing!");
                //}
            }
        }
    }
}
