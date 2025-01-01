using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class Heal : MonoBehaviour
{
    public int healAmount = 25; // Amount of health to restore

    private void OnTriggerEnter2D(Collider2D collision)
    {
        // Check if the object collided with has a PlayerController
        PlayerController player = collision.GetComponent<PlayerController>();
        if (player != null)
        {
            // Heal the player if their health is less than or equal to 80
            if (player.health <= 80)
            {
                player.health += healAmount;
                if (player.health > 100)
                {
                    player.health = 100; // Ensure health doesn't exceed 100
                }

                // Update health bar and provide feedback
                player.healthBar.SetHealth(player.health);

                Debug.Log("Player healed! Current health: " + player.health);

                // Optionally destroy the healing object after use
                //Destroy(gameObject);
            }
            Destroy(gameObject);
        }
    }
}