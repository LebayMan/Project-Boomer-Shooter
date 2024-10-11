using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sprite_Enemy : MonoBehaviour
{
    [Header("References")]

    public Animator animator;
    
    public GameObject Player { get => player; }
    [Header("Sight Values")]
    public float sightDistance = 20f;
    public float frontFieldOfView = 90f; // Field of view for the front detection
    public float rightFieldOfView = 90f; // Field of view for the right detection
    public float leftFieldOfView = 90f; // Field of view for the left detection
    public float backFieldOfView = 90f;
    public float eyeHeight;
    [Header("Detection Booleans")]
    public bool isPlayerInFront;
    public bool isPlayerToRight;
    public bool isPlayerToLeft;
    public bool isPlayerBehind;
    private GameObject player;
    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
    }

    // Update is called once per frame
    void Update()
    {
        DetectPlayerDirection();
        if(isPlayerBehind)
        {
            animator.Play("Back", 0, 0f);
        }
        if(isPlayerToRight)
        {
            animator.Play("Right", 0, 0f);
        }
        if(isPlayerToLeft)
        {
            animator.Play("Left", 0, 0f);
        }
        if(isPlayerInFront)
        {
            animator.SetTrigger("Front");
        }
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

            // Log the current direction for debugging
            Debug.Log($"Player Direction: Front({isPlayerInFront}), Right({isPlayerToRight}), Left({isPlayerToLeft}), Back({isPlayerBehind})");
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
