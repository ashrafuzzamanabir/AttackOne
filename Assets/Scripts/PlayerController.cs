using System.Collections;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public int health = 100;
    public float moveSpeed = 5f;
    public float jumpForce = 3f;
    [SerializeField] private LayerMask jumpableGround;
    public Animator animator;
    public Transform groundCheck;
    public float groundCheckRadius;

    audioManager audioManager;

    private Rigidbody2D rb;
    private CapsuleCollider2D coll;
    private int jumpCount = 0;
    private bool isFacingRight = true;
    private bool isDead = false;


    private GameManager gameManager;
    [SerializeField] public HealthBar healthBar;

    public bool IsRespawning { get; private set; } = false;
    public bool IsDead { get; private set; } = false;

    private void Awake()
    {
        audioManager = GameObject.FindGameObjectWithTag("Audio").GetComponent<audioManager>();
    }

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        coll = GetComponent<CapsuleCollider2D>();
        gameManager = FindObjectOfType<GameManager>();
        healthBar.SetMaxHealth(health);
    }

    void Update()
    {
        if (isDead || IsRespawning) return; // Prevent movement during respawn or death

        // Horizontal movement
        float move = Input.GetAxis("Horizontal");
        rb.velocity = new Vector2(move * moveSpeed, rb.velocity.y);

        // Flip sprite direction
        if (move > 0 && !isFacingRight)
            Flip();
        else if (move < 0 && isFacingRight)
            Flip();

        // Jump logic
        //if (Input.GetButtonDown("Jump") && jumpCount < 1)
        //{
        //    audioManager.PlaySFX(audioManager.jump);
        //    jumpCount++;
        //    rb.velocity = new Vector2(rb.velocity.x, jumpForce);
        //}

        if (Input.GetButtonDown("Jump") && jumpCount < 1)
        {
            audioManager.PlaySFX(audioManager.jump);
            jumpCount++;

            // Apply an upward force for the jump
            rb.AddForce(Vector2.up * jumpForce, ForceMode2D.Impulse);
        }

        if (IsGrounded())
        {
            jumpCount = 0;
            animator.SetBool("IsJumping", false);
        }
        else
        {
            animator.SetBool("IsJumping", true);
        }

        // Update animations
        animator.SetFloat("Speed", Mathf.Abs(move));
    }

    private bool IsGrounded()
    {
        //return Physics2D.BoxCast(
        //    coll.bounds.center,
        //    coll.bounds.size,
        //    0f,
        //    Vector2.down,
        //    0.1f,
        //    jumpableGround
        //);
        return Physics2D.OverlapCircle(groundCheck.position, groundCheckRadius, jumpableGround);
    }

    void Flip()
    {
        isFacingRight = !isFacingRight;
        Vector3 scale = transform.localScale;
        scale.x *= -1;
        transform.localScale = scale;
    }

    public void TakeDamage(int damage)
    {
        health -= damage;
        audioManager.PlaySFX(audioManager.hit);

        healthBar.SetHealth(health);

        if (health <= 0)
        {
            Die();
        }
        else
        {
            // Play hit animation
            animator.SetTrigger("Hit");
            StartCoroutine(ResetHitTrigger());
        }
    }

    private IEnumerator ResetHitTrigger()
    {
        yield return new WaitForSeconds(0.4f);
        animator.ResetTrigger("Hit");
    }

    public void ResetHealth()
    {
        health = 100;
        healthBar.SetMaxHealth(health);
    }

    public void Die()
    {
        if (isDead) return;
        audioManager.PlaySFX(audioManager.death);

        isDead = true;
        animator.SetTrigger("Die");

        rb.velocity = Vector2.zero;
        rb.isKinematic = true;
        coll.enabled = false;

        Debug.Log("Player has died!");
        StartCoroutine(RespawnAfterDelay(0.5f)); // Add delay for respawn
    }

    private IEnumerator RespawnAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);
        gameManager.PlayerDied();
    }

    public void Respawn(Vector2 respawnPosition)
    {
        StartCoroutine(RespawnSequence(respawnPosition));
    }

    private IEnumerator RespawnSequence(Vector2 respawnPosition)
    {
        // Set respawn-related flags
        IsRespawning = true;
        isDead = false;

        // Reset health and position
        health = 100;
        healthBar.SetMaxHealth(health);
        transform.position = respawnPosition;

        // Reactivate physics and controls
        rb.velocity = Vector2.zero;
        rb.isKinematic = false; // Ensure Rigidbody2D is dynamic again
        rb.constraints = RigidbodyConstraints2D.FreezeRotation; // Avoid any unwanted rotation
        coll.enabled = true;

        // Reset animations
        animator.ResetTrigger("Die");
        animator.SetTrigger("Respawn");
        animator.SetBool("IsJumping", false);

        yield return new WaitForSeconds(1f); // Short immunity period

        // Clear respawn flags and ensure player can move
        animator.ResetTrigger("Respawn");
        animator.SetFloat("Speed", 0);
        IsRespawning = false;
    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawSphere(groundCheck.position, groundCheckRadius);
    }

}
