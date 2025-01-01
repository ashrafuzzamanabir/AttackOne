using System.Collections;
using UnityEngine;

public class GroundEnemyBoss : MonoBehaviour
{
    [Header("Movement Parameters")]
    public float speed = 2f;
    public float patrolRange = 5f; // Distance to patrol left and right
    public int damage = 25;       // Damage to the player
    public int health = 150;      // Boss health
    private float leftBoundary;
    private float rightBoundary;
    private bool movingRight = true;

    [Header("Attack Parameters")]
    public float attackRadius = 2f; // Distance to detect and attack the player
    private GameObject player;
    private Coroutine damageCoroutine;

    [Header("Bounce Back Parameters")]
    public float bounceBackDistance = 1f;
    public float bounceBackTime = 0.2f;

    [Header("Animator & Effects")]
    public Animator animator;
    public GameObject bloodSplashParticle;

    private void Start()
    {
        // Set patrol boundaries based on the initial position
        leftBoundary = transform.position.x - patrolRange;
        rightBoundary = transform.position.x + patrolRange;
        player = GameObject.FindGameObjectWithTag("Player");
    }

    private void Update()
    {
        if (player == null) return;

        float distanceToPlayer = Vector2.Distance(transform.position, player.transform.position);

        if (distanceToPlayer <= attackRadius)
        {
            StopAndAttackPlayer();
        }
        else
        {
            Patrol();
        }

        Flip();
    }

    private void Patrol()
    {
        if (movingRight)
        {
            transform.position += Vector3.right * speed * Time.deltaTime;
            if (transform.position.x >= rightBoundary)
            {
                movingRight = false;
            }
        }
        else
        {
            transform.position += Vector3.left * speed * Time.deltaTime;
            if (transform.position.x <= leftBoundary)
            {
                movingRight = true;
            }
        }
    }

    private void StopAndAttackPlayer()
    {
        // Stop moving and stay at the current position
        transform.position = transform.position;

        // Play attack animation
        animator.SetTrigger("attack");

        // Start damaging the player over time
        if (damageCoroutine == null)
        {
            PlayerController playerController = player.GetComponent<PlayerController>();
            if (playerController != null)
            {
                damageCoroutine = StartCoroutine(DamagePlayerOverTime(playerController));
            }
        }
    }

    private void Flip()
    {
        if (movingRight)
        {
            transform.rotation = Quaternion.Euler(0, 0, 0);
        }
        else
        {
            transform.rotation = Quaternion.Euler(0, 180, 0);
        }
    }

    public void TakeDamage(int damage)
    {
        health -= damage;

        Debug.Log($"Enemy Health: {health}");

        if (health <= 0)
        {
            Die();
        }
        else
        {
            animator.SetTrigger("attacked");
        }
    }

    private void Die()
    {
        Debug.Log("Enemy Boss Died!");
        animator.SetTrigger("death");
        StartCoroutine(DestroyAfterDeath());
    }

    private IEnumerator DestroyAfterDeath()
    {
        yield return new WaitForSeconds(0.5f); // Adjust based on death animation length
        Destroy(gameObject);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            // Spawn blood effect
            Vector3 collisionPoint = collision.ClosestPoint(transform.position);
            Instantiate(bloodSplashParticle, collisionPoint, Quaternion.identity);

            // Apply bounce-back effect
            Vector2 bounceDirection = (transform.position - collision.transform.position).normalized;
            StartCoroutine(BounceBack(bounceDirection));
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            StopDamageCoroutine();
        }
    }

    private void StopDamageCoroutine()
    {
        if (damageCoroutine != null)
        {
            StopCoroutine(damageCoroutine);
            damageCoroutine = null;
        }
    }

    private IEnumerator DamagePlayerOverTime(PlayerController playerController)
    {
        while (playerController != null && !playerController.IsDead && !playerController.IsRespawning)
        {
            playerController.TakeDamage(damage);
            yield return new WaitForSeconds(0.5f);
        }

        StopDamageCoroutine();
    }

    private IEnumerator BounceBack(Vector2 direction)
    {
        Vector2 startPosition = transform.position;
        Vector2 targetPosition = startPosition + direction * bounceBackDistance;

        float elapsedTime = 0f;
        while (elapsedTime < bounceBackTime)
        {
            transform.position = Vector2.Lerp(startPosition, targetPosition, elapsedTime / bounceBackTime);
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        transform.position = targetPosition;
    }
}
