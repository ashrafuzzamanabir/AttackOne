using UnityEngine;

public class BulletController : MonoBehaviour
{
    private LayerMask targetLayers; // Layers the bullet interacts with

    /// <summary>
    /// Sets up the bullet with target layers.
    /// </summary>
    public void Setup(LayerMask layers)
    {
        targetLayers = layers;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        // Check if the collided object is on the target layers
        if (((1 << collision.gameObject.layer) & targetLayers) != 0)
        {
            Destroy(gameObject); // Destroy the bullet upon collision
        }
    }
}
