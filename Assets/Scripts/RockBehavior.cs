using System.Collections;
using UnityEngine;

public class Rock : MonoBehaviour
{
    [SerializeField] private LayerMask platformLayer;// Layer for the platform
    [SerializeField] private LayerMask spikeLayer; // Layer for the platform

    [SerializeField] private float destroyDelay = 2f; // Delay before rock disappears after hitting platform
    public Transform respawnPoint;// Respawn position

    public int damageToPlayer = 25; // Amount of health to reduce when hit



    public Animator animator;
    

    private GameManager gameManager;

    private void Start()
    {
        animator = GetComponent<Animator>();
        gameManager = FindObjectOfType<GameManager>();
        if (gameManager == null)
        {
            Debug.LogError("GameManager not found in the scene!");
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {

        // Check if the rock collides with an object on the platform layer
        if (((1 << collision.gameObject.layer) & (platformLayer | spikeLayer)) != 0)
        {
            animator.SetTrigger("hitplatform");
            Debug.Log($"Rock hit {collision.gameObject.name}, will be destroyed in {destroyDelay} seconds.");
            StartCoroutine(DestroyRock());
        }
        PlayerController player = collision.gameObject.GetComponent<PlayerController>();
        //if (player != null)
        //{
        //    player.Die(); // Trigger player's death animation

        //    if (respawnPoint != null)
        //    {
        //        collision.gameObject.transform.position = respawnPoint.position; // Respawn player
        //    }

        //    gameManager?.PlayerDied(); // Notify GameManager about the death
        //}
        if (player != null)
        {
            player.TakeDamage(damageToPlayer);
            animator.SetTrigger("hitplatform");
            StartCoroutine(DestroyRock());
        }

    }

    private IEnumerator DestroyRock()
    {
        yield return new WaitForSeconds(destroyDelay);
        Destroy(gameObject); // Destroy the rock
        animator.ResetTrigger("hitplatform");
    }
}
