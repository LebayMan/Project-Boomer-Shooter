using UnityEngine;

public class GrenadeAmmo : MonoBehaviour
{
    [Header("Grenade Settings")]
    public float explosionRadius = 5f;
    public float explosionForce = 700f;
    public Vector3 explosionOffset;
    public LayerMask enemyLayer;
    public LayerMask obstacleLayer; // The layer(s) that trigger the explosion
    public float damage = 10f;

    public GameObject explosionEffectPrefab; // Prefab for explosion effect

    private bool hasExploded = false;

    private void OnCollisionEnter(Collision collision)
    {
        // Check if the grenade collided with an obstacle or enemy layer
        if (((1 << collision.gameObject.layer) & obstacleLayer) != 0 || 
            ((1 << collision.gameObject.layer) & enemyLayer) != 0)
        {
            Explode();
        }
    }

    private void Explode()
    {
        if (hasExploded) return;
        hasExploded = true;

        // Instantiate explosion effect
        if (explosionEffectPrefab != null)
        {
            Instantiate(explosionEffectPrefab, transform.position+ explosionOffset, Quaternion.identity);
        }

        // Apply explosion force and damage to enemies within the radius
        Collider[] colliders = Physics.OverlapSphere(transform.position, explosionRadius, enemyLayer);
        foreach (Collider nearbyObject in colliders)
        {
            Rigidbody rb = nearbyObject.GetComponent<Rigidbody>();
            if (rb != null)
            {
                rb.AddExplosionForce(explosionForce, transform.position, explosionRadius);
            }

            // Apply damage to enemies
            CollisonEnemy enemy = nearbyObject.GetComponent<CollisonEnemy>();
            if (enemy != null)
            {
                enemy.Hit(damage);
            }
        }

        // Destroy the grenade after explosion
        Destroy(gameObject);
    }

    private void OnDrawGizmosSelected()
    {
        // Draw a sphere to visualize the explosion radius
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, explosionRadius);
    }
}
