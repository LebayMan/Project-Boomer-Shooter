using UnityEngine;
using UnityEngine.AI;

public class Enemy : MonoBehaviour
{
    [Header("Enemy Info")]
    public float Health;
    public float MaxHealth = 100;

    [Header("References")]
    public GameObject Explosion;
    public Animator animator;
    public NavMeshAgent Agent { get => agent;}
    public NavMeshAgent agent;
    
    public  GameObject Player { get => player;}
    [Header("Explosion Settings")]
    public Vector3 explosionOffset;
    private float nextPlayTime = 0f;
    private StateMachine stateMachine;
    [Header("Patrol Time")]
    public float waitTimer;
    
    [SerializeField]
    private string currentState;
    public Path path;
    private GameObject player;
    [Header("Sight Value")]
    public float sightDistance = 20f;
    public float fieldofview = 85f;
    public float eyeHeight;
    [Header ("Weapon Values")]
    public Transform gunBarrel;
    [Range(0.1f,10f)]
    public float fireRate;
    

    private void Start()
    {
        stateMachine = GetComponent<StateMachine>();
        //agent = GetComponent<NavMeshAgent>();
        stateMachine.Initialise();
        Health = MaxHealth;
        player = GameObject.FindGameObjectWithTag("Player");
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
    CanSeePlayer();
    currentState = stateMachine.activestate.ToString();
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
    public bool CanSeePlayer()
    {
    if (player != null)
    {
        
        if (Vector3.Distance(transform.position, player.transform.position) < sightDistance)
        {
            
            Vector3 targetDirection = player.transform.position - transform.position - Vector3.up * eyeHeight;
            
            float angleToPlayer = Vector3.Angle(targetDirection, transform.forward);
            
            if (angleToPlayer >= -fieldofview && angleToPlayer <= fieldofview)
                {
                
                Ray ray = new Ray(transform.position + (Vector3.up * eyeHeight), targetDirection);
                RaycastHit hitInfo = new RaycastHit();
                if(Physics.Raycast(ray,out hitInfo, sightDistance))
                    {
                    if(hitInfo.transform.gameObject == player)
                        {
                            Debug.DrawRay(ray.origin, ray.direction * sightDistance);
                            return true;
                        }
                    }
                }
            }
        }
        return false; 
    }
}


