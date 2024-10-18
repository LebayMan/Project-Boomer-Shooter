using UnityEngine;

public class Bullets : MonoBehaviour
{
    private Collider bulletCollider;

    [SerializeField]
    private int damageAmount = 10; // Damage amount set in the Inspector

    // Public layer for obstacles
    [SerializeField]
    private LayerMask obstacleLayer; // Selectable in the Unity Inspector

    private void Start()
    {
        bulletCollider = GetComponent<Collider>();
    }

    private void OnCollisionEnter(Collision collision)
    {
        Transform hitTransform = collision.transform;

        // Check if the collided object is the player
        if (collision.gameObject.layer == LayerMask.NameToLayer("Player"))
        {
            Debug.Log("Hit Player");

            // Get the Health script from the player object
            Health playerHealth = hitTransform.GetComponent<Health>();
            if (playerHealth != null)
            {
                // Call the Damage method with the specified damage amount
                playerHealth.UpdateHealth(-damageAmount); // Use the damage amount set in the Inspector
            }

            // Destroy the bullet after applying damage
            Destroy(gameObject);
        }
        // Check if the collided object is an obstacle
        else if (((1 << collision.gameObject.layer) & obstacleLayer) != 0)
        {
            Debug.Log("Hit Obstacle");

            // Destroy the bullet when it hits an obstacle
            Destroy(gameObject);
        }
    }
}
