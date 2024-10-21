using UnityEngine;
using UnityEngine.AI;

public class Enemy : MonoBehaviour
{
    [Header("Enemy Info")]
    public float Health;
    public float MaxHealth = 100;
    public float turnSpeed = 0.1f;
    public float timeToSearch = 1f;

    [Header("References")]
    public GameObject Explosion;
    public Animator animator;
    public NavMeshAgent Agent { get => agent; }
    public NavMeshAgent agent;
    public GameObject gameObject1;

    public GameObject Player { get => player; }
    [Header("Explosion Settings")]
    public Vector3 explosionOffset;
    [Header("Bullet Settings")]
    public float bulletSpeed = 40f;
    private float nextPlayTime = 0f;
    private StateMachine stateMachine;
    [Header("Shooting Range")]
    public float shootingRange = 10f;
    public float minimumRange =5f;
    [Header("Patrol Time")]
    public float waitTimer;

    [SerializeField]
    private string currentState;
    public Path path;
    private GameObject player;

    [Header("Sight Values")]
    public float sightDistance = 20f;
    public float frontFieldOfView = 90f; // Field of view for the front detection
    public float rightFieldOfView = 90f; // Field of view for the right detection
    public float leftFieldOfView = 90f; // Field of view for the left detection
    public float backFieldOfView = 90f; // Field of view for the back detection
    public float eyeHeight;

    [Header("Weapon Values")]
    public Transform gunBarrel;
    [Range(0.1f, 10f)]
    public float fireRate;

    [Header("Detection Booleans")]
    public bool isPlayerInFront;
    public bool isPlayerToRight;
    public bool isPlayerToLeft;
    public bool isPlayerBehind;
    [Header("Stun Settings")]
    public float stunDuration = 3f; // Public stun duration
    public float StuntTime;
    public bool Stunt;
    

private GameMaster gameMaster;

private void Start()
{
    stateMachine = GetComponent<StateMachine>();
    stateMachine.Initialise();
    Health = MaxHealth;
    player = GameObject.FindGameObjectWithTag("Player");
    gameMaster = GameObject.FindGameObjectWithTag("GameMaster").GetComponent<GameMaster>();
}


    public void Hit(float damage)
    {
        Health -= damage;
        animator.Play("Hit", 0, 0f);
        stateMachine.ChangeState(new StunState(this, stunDuration,damage,stateMachine.playerTransform));
        Stunt = true;
        if (Health <= 0)
        {
            gameMaster.EnemyAlive--;
            Die();
        }
    }
    private void FixedUpdate()
    {
            if(Stunt)
            {
                StuntTime += 1;
            }
    }
    private void Update()
    {
        CanSeePlayer();
        DetectPlayerDirection();
        currentState = stateMachine.activeState.ToString();

        if (Health <= 0)
        {
            Die();
        }
        if(isPlayerBehind)
        {
            animator.Play("Back", 0, 0f);
        }
        if(isPlayerToRight)
        {
            animator.Play("Left", 0, 0f);
        }
        if(isPlayerToLeft)
        {
            animator.Play("Right", 0, 0f);
        }
        if(isPlayerInFront)
        {
            animator.SetTrigger("Front");
        }
    }

    private void Die()
    {
        if (Explosion != null)
        {
            Instantiate(Explosion, transform.position + explosionOffset, transform.rotation);
        }
        Destroy(gameObject1);
    }

public bool CanSeePlayer()
{
    if (player != null && !Stunt)
    {
        Vector3 targetDirection = player.transform.position - transform.position - Vector3.up * eyeHeight;

        if (Vector3.Distance(transform.position, player.transform.position) < sightDistance)
        {
            // Calculate the angle between the enemy's forward direction and the player
            float angleToPlayer = Vector3.Angle(targetDirection, transform.forward);

            // Only detect the player if they're within the front field of view
            if (angleToPlayer >= -frontFieldOfView / 2 && angleToPlayer <= frontFieldOfView / 2)
            {
                Ray ray = new Ray(transform.position + Vector3.up * eyeHeight, targetDirection);
                RaycastHit hitInfo;

                // Perform a raycast to check if there's an obstruction between the enemy and the player
                if (Physics.Raycast(ray, out hitInfo, sightDistance))
                {
                    if (hitInfo.transform.gameObject == player)
                    {
                        // Debug line to visualize the ray
                        Debug.DrawRay(ray.origin, ray.direction * sightDistance, Color.red);
                        return true; // Player detected within front field of view
                    }
                }
            }
        }
    }
    return false; // Player not detected
}

    private void DetectPlayerDirection()
    {
        if (player != null)
        {
            Vector3 directionToPlayer = player.transform.position - transform.position;
            directionToPlayer.y = 0; // Ignore height differences
            float angleToPlayer = Vector3.SignedAngle(transform.forward, directionToPlayer, Vector3.up);

            // Check and set the boolean values based on player's relative angle
            isPlayerInFront = (angleToPlayer > -frontFieldOfView / 2 && angleToPlayer <= frontFieldOfView / 2);
            isPlayerToRight = (angleToPlayer > frontFieldOfView / 2 && angleToPlayer <= 180 - backFieldOfView / 2);
            isPlayerToLeft = (angleToPlayer > -(180 - backFieldOfView / 2) && angleToPlayer <= -frontFieldOfView / 2);
            isPlayerBehind = (angleToPlayer <= -180 + backFieldOfView / 2 || angleToPlayer >= 180 - backFieldOfView / 2);

            // Debug visualization for all directions
            Vector3 eyePosition = transform.position + Vector3.up * eyeHeight;
            Debug.DrawRay(eyePosition, transform.forward * sightDistance, Color.green); // Front
            Debug.DrawRay(eyePosition, Quaternion.Euler(0, frontFieldOfView / 2, 0) * transform.forward * sightDistance, Color.blue); // Right
            Debug.DrawRay(eyePosition, Quaternion.Euler(0, -frontFieldOfView / 2, 0) * transform.forward * sightDistance, Color.blue); // Left
            Debug.DrawRay(eyePosition, -transform.forward * sightDistance, Color.yellow); // Back

        }
    }

    private void OnDrawGizmos()
    {
        Vector3 eyePosition = transform.position + Vector3.up * eyeHeight;
        Gizmos.color = Color.yellow;

        // Draw FOV lines for debugging
        Vector3 leftBoundary = Quaternion.Euler(0, -frontFieldOfView / 2, 0) * transform.forward * sightDistance;
        Vector3 rightBoundary = Quaternion.Euler(0, frontFieldOfView / 2, 0) * transform.forward * sightDistance;

        Gizmos.DrawLine(eyePosition, eyePosition + leftBoundary); // Front left FOV
        Gizmos.DrawLine(eyePosition, eyePosition + rightBoundary); // Front right FOV

        // Draw circle to represent sight distance
        Gizmos.DrawWireSphere(eyePosition, sightDistance);
    }
}
