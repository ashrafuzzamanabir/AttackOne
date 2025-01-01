using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class RespawnManager : MonoBehaviour
{
    [SerializeField] private Transform respawnPoint; // Respawn point for the player
    [SerializeField] private string finishingSceneName; // Name of the finishing scene
    [SerializeField] private int maxRespawns = 3; // Maximum number of respawns allowed

    private int respawnCount = 0; // Tracks how many times the player has respawned

    /// <summary>
    /// Handles player respawn or loads the finishing scene if the limit is reached.
    /// </summary>
    public void HandleRespawn(GameObject player)
    {
        if (player == null)
        {
            Debug.LogError("Player object is null! Cannot respawn.");
            return;
        }

        // Check if the respawn limit has been reached
        if (respawnCount < maxRespawns)
        {
            RespawnPlayer(player);
            respawnCount++;
            Debug.Log($"Player Respawned {respawnCount}/{maxRespawns} times.");
        }
        else
        {
            Debug.Log("Respawn limit reached. Loading finishing scene...");
            LoadFinishingScene(); // Load the finishing scene
        }
    }

    /// <summary>
    /// Respawns the player at the respawn point.
    /// </summary>
    public void RespawnPlayer(GameObject player)
    {
        if (respawnPoint == null)
        {
            Debug.LogError("Respawn point is not set! Cannot respawn player.");
            return;
        }

        // Reset player position
        player.transform.position = respawnPoint.position;

        // Reset physics
        Rigidbody2D rb = player.GetComponent<Rigidbody2D>();
        if (rb != null)
        {
            rb.velocity = Vector2.zero; // Reset velocity
            rb.angularVelocity = 0f; // Reset angular velocity
        }

        // Reset animations
        PlayerController playerController = player.GetComponent<PlayerController>();
        if (playerController != null && playerController.animator != null)
        {
            playerController.Respawn(respawnPoint.position);
            playerController.animator.ResetTrigger("Die"); // Clear "Die" trigger
            playerController.animator.SetTrigger("Respawn"); // Trigger Respawn animation
            playerController.animator.SetFloat("Speed", 0);
            playerController.animator.SetBool("IsJumping", false);
            playerController.ResetHealth(); // Reset player's health
        }
        else
        {
            Debug.LogWarning("PlayerController or Animator not found. Skipping animation reset.");
        }
    }

    /// <summary>
    /// Loads the finishing scene when the respawn limit is reached.
    /// </summary>
    private void LoadFinishingScene()
    {
        if (string.IsNullOrEmpty(finishingSceneName))
        {
            Debug.LogError("Finishing scene name is not set! Cannot load scene.");
            return;
        }

        SceneManager.LoadScene(finishingSceneName);
    }
}
