using UnityEngine;

public class SpikeTrap : MonoBehaviour
{
    [SerializeField] private LayerMask playerLayer; // Layer for the player
    [SerializeField] private Transform respawnPoint; // Respawn point for the player

    private GameManager gameManager;

    private void Start()
    {
        gameManager = FindObjectOfType<GameManager>();
        if (gameManager == null)
        {
            Debug.LogError("GameManager not found in the scene!");
        }
    }

    //private void OnCollisionEnter2D(Collision2D collision)
    //{
    //    if (((1 << collision.gameObject.layer) & playerLayer) != 0)
    //    {
    //        PlayerController player = collision.gameObject.GetComponent<PlayerController>();
    //        if (player != null)
    //        {
    //            player.Die(); // Trigger player's death animation

    //            if (respawnPoint != null)
    //            {
    //                collision.gameObject.transform.position = respawnPoint.position; // Respawn player
    //            }

    //            gameManager?.PlayerDied(); // Notify GameManager about the death
    //        }
    //    }
    //}

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (((1 << collision.gameObject.layer) & playerLayer) != 0)
        {
            PlayerController player = collision.gameObject.GetComponent<PlayerController>();
            if (player != null)
            {
                player.Die();
            }
        }
    }
}
