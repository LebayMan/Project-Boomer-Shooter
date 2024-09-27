using UnityEngine;

public class Enemy : MonoBehaviour
{
    [Header("Enemy Info")]
    public float Health;
    public float MaxHealth = 100;

    [Header("References")]
    public GameObject Explosion;
    public Animator animator;
    
    [Header("Explosion Settings")]
    public Vector3 explosionOffset;
    private float nextPlayTime = 0f;

    private void Start()
    {
        Health = MaxHealth;
    }

    public void Hit(float damage)
    {
        Health -= damage;
        animator.Play("Hit", 0, 0f);
        if (Health <= 0)
        {
            Die();
        }
    }

private void Update()
{
    if (Time.time >= nextPlayTime)
    {
        animator.Play("Enemy", 0, 0f);
        
        // Set the next play time to a random value between 5 and 7 seconds from now
        nextPlayTime = Time.time + Random.Range(5f, 7f);
    }

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
