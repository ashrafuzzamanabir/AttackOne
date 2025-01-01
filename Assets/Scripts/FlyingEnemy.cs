using System.Collections;
using UnityEngine;


public class FlyingEnemy : MonoBehaviour
{
    public float speed = 2f;
    public float patrolRadius = 5f;
    public int damage = 25;       // Damage to player
    public int health = 100;     // Health of the enemy
    private Vector2 initialPosition;
    private GameObject player;
    private Coroutine damageCoroutine;

    public float bounceBackDistance = 1f; // Distance to move back after hitting the player
    public float bounceBackTime = 0.2f;   // Time to complete the bounce-back motion
    public Animator animator;

    public GameObject bloodSplashParticle;


    void Start()
    {
        initialPosition = transform.position;
        player = GameObject.FindGameObjectWithTag("Player");
    }

    void Update()
    {
        if (player == null)
            return;

        float distanceToPlayer = Vector2.Distance(transform.position, player.transform.position);

        if (distanceToPlayer <= patrolRadius)
        {
            Chase();
        }
        else
        {
            ReturnToInitialPosition();
        }

        Flip();
    }

    private void Chase()
    {
        transform.position = Vector2.MoveTowards(transform.position, player.transform.position, speed * Time.deltaTime);
    }

    private void ReturnToInitialPosition()
    {
        transform.position = Vector2.MoveTowards(transform.position, initialPosition, speed * Time.deltaTime);
    }

    private void Flip()
    {
        if (player != null && transform.position.x > player.transform.position.x)
            transform.rotation = Quaternion.Euler(0, 180, 0);
        else
            transform.rotation = Quaternion.Euler(0, 0, 0);

    }

    public void TakeDamage(int damage)
    {

        health -= damage;

        Debug.Log($"Enemy Health: {health}");

        if (health <= 0)
        {
            Die();
        }
        //else
        //{
        //    // Play hit animation
        //    animator.SetTrigger("attacked");
        //    StartCoroutine(ResetattackedTrigger());
        //}

    }

    //private IEnumerator ResetattackedTrigger()
    //{
    //    yield return new WaitForSeconds(0.4f);
    //    animator.ResetTrigger("attacked");
    //    //animator.SetBool("speed", true);

    //}

    private void Die()
    {
        Debug.Log("Enemy Died!");
        // Trigger death animation
        animator.SetTrigger("flydeath");
        // Start coroutine to destroy game object after animation
        StartCoroutine(DestroyAfterDeathAnimation());
    }

    private IEnumerator DestroyAfterDeathAnimation()
    {
        // Wait until the flydeath animation state is entered
        yield return new WaitForSeconds(0.2f);
        animator.ResetTrigger("flydeath");
        // Destroy the game object
        Destroy(gameObject);

    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            if (damageCoroutine == null)
            {
                PlayerController playerController = collision.GetComponent<PlayerController>();
                if (playerController != null)
                {
                    damageCoroutine = StartCoroutine(DamagePlayerOverTime(playerController));
                }
            }
        }

        // Instantiate blood splash particle at the collision point
        Vector3 collisionPoint = collision.ClosestPoint(transform.position);
        Instantiate(bloodSplashParticle, collisionPoint, Quaternion.identity);


        // Bounce back logic
        Vector2 bounceDirection = (transform.position - collision.transform.position).normalized;
        StartCoroutine(BounceBack(bounceDirection));
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            StopDamageCoroutine();
        }
    }
    // New method to stop the damage coroutine
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
