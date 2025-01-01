using System.Collections; 
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    public float speed = 20f;
    public int damage = 40;

    private Rigidbody2D rb;
    private float direction;

    public Animator animator;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        if (rb == null)
        {
            Debug.LogError("Rigidbody2D is missing from the bullet!");
            return;
        }

        rb.velocity = transform.right * speed;
    }

    public void Setup(float direction)
    {
        this.direction = direction;
        FlipBullet();
    }

    void FlipBullet()
    {
        // Check if the bullet needs to be flipped based on direction
        if (direction < 0 && transform.rotation.eulerAngles.y == 0)
        {
            // Rotate the bullet to face the left direction
            transform.Rotate(0f, 180f, 0f);
        }
        else if (direction > 0 && transform.rotation.eulerAngles.y != 0)
        {
            // Reset rotation to face the right direction
            transform.Rotate(0f, 180f, 0f);
        }
    }



    void OnCollisionEnter2D(Collision2D collision)
    {
        Debug.Log($"Bullet collided with: {collision.gameObject.name}, Layer: {LayerMask.LayerToName(collision.gameObject.layer)}");

        // Check if the collision is with an object in the "Enemy" layer
        if (collision.gameObject.layer == LayerMask.NameToLayer("enemy"))
        {
            Enemy enemy = collision.gameObject.GetComponent<Enemy>();
            if (enemy != null)
            {
                enemy.TakeDamage(damage);
                Debug.Log($"Damaged {collision.gameObject.name}. Remaining health: {enemy.health}");
            }

            // Destroy the bullet
            Destroy(gameObject);
        }
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("FlyEnemy"))
        {
            FlyingEnemy enemy = collision.GetComponent<FlyingEnemy>();
            if (enemy != null)
            {
                enemy.TakeDamage(damage); // Apply damage to the enemy

            }
            Destroy(gameObject); // Destroy the bullet
        }
        // Check if the bullet hit an enemy
        //if (collision.CompareTag("bossenemy"))
        //{
        //    EnemyHealth enemyHealth = collision.GetComponent<EnemyHealth>();
        //    if (enemyHealth != null)
        //    {
        //        enemyHealth.TakeDamage(damage); // Call the TakeDamage method
        //    }
        //    Destroy(gameObject); // Destroy the bullet
        //}
    }

}
