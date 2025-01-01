using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Destroyer : MonoBehaviour
{
    public float timeTilDestroy = 5f;
    [SerializeField] private LayerMask playerLayer; // Layer for the player
    public int damageToPlayer = 25; // Amount of health to reduce when hit

    private GameManager gameManager;
    public Animator animator;



    private void Start()
    {
        gameManager = FindObjectOfType<GameManager>();
        // Destroy the object after the specified time
        Destroy(gameObject, timeTilDestroy);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        // Check if the collided object is in the player layer
        if (((1 << collision.gameObject.layer) & playerLayer) != 0)
        {
            PlayerController player = collision.gameObject.GetComponent<PlayerController>();
            if (player != null)
            {
                // Reduce the player's health
                player.TakeDamage(damageToPlayer);
                

            }


        }
    }
}
