using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using UnityEngine.SceneManagement;


public class MovingLaser : MonoBehaviour
{
    [SerializeField] private float defDistanceRay = 100f; // Default distance for the laser
    public Transform laserFirePoint; // Point where the laser originates
    public LineRenderer m_lineRenderer; // Line Renderer component
    public BoxCollider2D laserCollider; // Collider for the laser
    [SerializeField] private LayerMask collisionLayers; // Layers the laser can collide with
    [SerializeField] private float rotationSpeed = 30f; // Speed of rotation in degrees per second

    private Transform m_transform; // Cached Transform component

    private void Awake()
    {
        m_transform = GetComponent<Transform>();
    }

    private void Update()
    {
        RotateLaserContainer();
        ShootLaser();
    }

    void RotateLaserContainer()
    {
        // Rotate the container along the Z-axis
        m_transform.Rotate(0, 0, rotationSpeed * Time.deltaTime);
    }

    void ShootLaser()
    {
        if (laserFirePoint == null || m_lineRenderer == null || laserCollider == null)
        {
            Debug.LogWarning("Missing components. Ensure laserFirePoint, m_lineRenderer, and laserCollider are assigned.");
            return;
        }

        // Check if the laser hits something within the specified layers
        RaycastHit2D _hit = Physics2D.Raycast(laserFirePoint.position, laserFirePoint.right, defDistanceRay, collisionLayers);
        Vector3 endPos; // Endpoint of the laser

        if (_hit.collider != null)
        {
            // Laser hits an object
            endPos = _hit.point;
            Debug.Log("Laser hit: " + _hit.collider.name);

            // Check if the hit object belongs to the player layer or has the "Player" tag
            if (_hit.collider.CompareTag("Player"))
            {
                PlayerController playerHealth = _hit.collider.GetComponent<PlayerController>();
                if (playerHealth != null)
                {
                    playerHealth.TakeDamage(25); // Decrease health by 25
                    Debug.Log($"Player health decreased. Current health: {playerHealth.health}");
                }
                else
                {
                    Debug.LogWarning("PlayerController component not found on the Player object.");
                }
            }
        }
        else
        {
            // Laser does not hit anything; extend to default distance
            endPos = (Vector2)laserFirePoint.position + (Vector2)(laserFirePoint.right * defDistanceRay);
            Debug.Log("Laser did not hit anything, extending to default distance.");
        }

        Draw2DRay(laserFirePoint.position, endPos);
        UpdateCollider(laserFirePoint.position, endPos);
    }


    void Draw2DRay(Vector3 startPos, Vector3 endPos)
    {
        if (m_lineRenderer != null)
        {
            m_lineRenderer.enabled = true; // Ensure the LineRenderer is enabled

            // Adjust Z position for both start and end points
            startPos.z = 0;
            endPos.z = -10;

            m_lineRenderer.SetPosition(0, startPos); // Start of the laser
            m_lineRenderer.SetPosition(1, endPos);   // End of the laser

            Debug.Log("Drawing laser from " + startPos + " to " + endPos);
        }
        else
        {
            Debug.LogWarning("LineRenderer component is missing.");
        }
    }

    void UpdateCollider(Vector3 startPos, Vector3 endPos)
    {
        // Calculate the length and position of the collider
        Vector2 direction = (endPos - startPos).normalized;
        float distance = Vector2.Distance(startPos, endPos);

        // Center of the collider
        laserCollider.offset = (Vector2)(startPos + endPos) / 2f - (Vector2)transform.position;

        // Update collider size
        laserCollider.size = new Vector2(distance, 0.1f); // Adjust height as needed for thickness

        // Rotate the collider to align with the laser
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        laserCollider.transform.rotation = Quaternion.Euler(0, 0, angle);

        Debug.Log("Updated collider size and position.");
    }
}


