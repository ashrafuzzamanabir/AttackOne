using UnityEngine;

public class Weapon : MonoBehaviour
{
    public Transform Firepoint; // Position from where bullets are fired
    public GameObject bulletPrefab; // Prefab for the bullet

    [SerializeField] private LayerMask playerLayer;

    [SerializeField] private LayerMask Layer1; // Layer for target objects
    [SerializeField] private LayerMask Layer2; // Layer for target objects
    [SerializeField] private LayerMask Layer3; // Layer for target objects

    audioManager audioManager;
    private GameManager gameManager; // Reference to the GameManager

    private void Awake()
    {
        audioManager = GameObject.FindGameObjectWithTag("Audio").GetComponent<audioManager>();
    }

    private void Start()
    {
        gameManager = FindObjectOfType<GameManager>(); // Find the GameManager in the scene
        if (gameManager == null)
        {
            Debug.LogError("GameManager not found in the scene!");
        }
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.X))
        {
            Shoot();
        }
    }

    void Shoot()
    {
        GameObject bullet = Instantiate(bulletPrefab, Firepoint.position, Firepoint.rotation);
        audioManager.PlaySFX(audioManager.fire);

        BulletController bulletController = bullet.AddComponent<BulletController>();
        bulletController.Setup(Layer1 | Layer2 | Layer3); // Pass combined layers to the bullet

        // Get the BulletController or Bullet script
        Bullet bulletScript = bullet.GetComponent<Bullet>();
        if (bulletScript != null)
        {
            // Pass the player's facing direction to the bullet
            float direction = transform.localScale.x > 0 ? 1f : -1f;
            bulletScript.Setup(direction);
        }

        Destroy(bullet, 3f); // Destroy the bullet after 3 seconds if it doesn't collide
    }

    // Global collision handler for bullets
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (((1 << collision.gameObject.layer) & playerLayer)== 0)
        {
            Debug.Log("Bullet collided with player, ignoring destruction.");
            return; // Ignore collision with player
        }

        Debug.Log($"Bullet collided with: {collision.gameObject.name}, destroying bullet.");
        Destroy(collision.gameObject); // Destroy the bullet upon collision with non-player objects
    }
}
