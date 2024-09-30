using UnityEngine;

public class Bullets : MonoBehaviour
{
    private Collider bulletCollider;

    private void Start()
    {
        bulletCollider = GetComponent<Collider>();
    }

    private void OnCollisionEnter(Collision collision)
    {
        Transform hitTransform = collision.transform;

        // Check if the collided object has the "Player" tag
        if (collision.gameObject.layer == LayerMask.NameToLayer("Player"))
        {
            Debug.Log("Hit Player");

            // Get the Health script from the player object
            Health playerHealth = hitTransform.GetComponent<Health>();
            if (playerHealth != null)
            {
                // Call the Damage method with the desired damage amount
                playerHealth.UpdateHealth(-10); // Replace 10f with your desired damage amount
            }

            // Destroy the bullet after applying damage
            Destroy(gameObject);
        }
        else
        {
            // If the bullet hits anything else, destroy it
            Destroy(gameObject);
        }
    }
}
