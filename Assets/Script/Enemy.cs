using UnityEngine;

public class Enemy : MonoBehaviour
{
    [Header("Enemy Info")]
    public float Health;
    public float MaxHealth = 100;

    [Header("References")]
    public GameObject Explosion;
    
    [Header("Explosion Settings")]
    public Vector3 explosionOffset; // This allows you to adjust the explosion position

    private void Start()
    {
        Health = MaxHealth;
    }

    public void Hit(float damage)
    {
        Health -= damage;
        if (Health <= 0)
        {
            Die();
        }
    }

    private void Update()
    {
        if (Health <= 0)
        {
            Die();
        }
    }

    private void Die()
    {
        // Instantiate the explosion with an adjustable offset
        if (Explosion != null)
        {
            Instantiate(Explosion, transform.position + explosionOffset, transform.rotation);
        }

        // Destroy the enemy GameObject
        Destroy(gameObject);
    }
}
