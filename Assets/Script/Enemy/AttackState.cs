using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackState : BaseState
{
    private float moveTimer;
    private float losePlayerTimer;
    private float shotTimer;
    
    
    public override void Enter()
    {
        
    }
    public override void Exit()
    {

    }
public void Shoot()
{
    
    Debug.Log("SHOOT");
    Transform gunbarrel = enemy.gunBarrel;

    // Instantiate the bullet
    GameObject bullet = GameObject.Instantiate(Resources.Load("Prefabs/Bullet") as GameObject, gunbarrel.position, enemy.transform.rotation);

    // Adjust the bullet's rotation to be rotated 90 degrees
    bullet.transform.rotation = Quaternion.Euler(bullet.transform.eulerAngles.x + 90, bullet.transform.eulerAngles.y, bullet.transform.eulerAngles.z);

    // Calculate the shooting direction
    Vector3 shootDirection = (enemy.Player.transform.position - gunbarrel.transform.position).normalized;

    // Add a slight random deviation for more realistic shooting
    shootDirection = Quaternion.Euler(Random.Range(-1f, 1f), Random.Range(-1f, 1f), 0) * shootDirection;

    // Apply velocity to the bullet
    Rigidbody bulletRigidbody = bullet.GetComponent<Rigidbody>();
    bulletRigidbody.velocity = shootDirection * enemy.bulletSpeed;

    shotTimer = 0;

}
public override void Perform()
{
        if (enemy.CanSeePlayer())
        {
            losePlayerTimer = 0;
            moveTimer += Time.deltaTime;
            shotTimer += Time.deltaTime;
            float distanceToPlayer = Vector3.Distance(enemy.transform.position, enemy.Player.transform.position);
            enemy.transform.LookAt(enemy.Player.transform);
            //Vector3 directionToPlayer = (enemy.Player.transform.position - enemy.transform.position).normalized;
            //Quaternion targetRotation = Quaternion.LookRotation(directionToPlayer);
            //enemy.transform.rotation = Quaternion.RotateTowards(enemy.transform.rotation, targetRotation, enemy.turnSpeed * Time.deltaTime);

            if (distanceToPlayer > enemy.shootingRange)
            {
                // Move closer to the player if the enemy is too far away
                enemy.Agent.SetDestination(enemy.Player.transform.position);
            }
            else if (distanceToPlayer < enemy.minimumRange)
            {
            // Move back to maintain minimum distance
            Vector3 directionAwayFromPlayer = (enemy.transform.position - enemy.Player.transform.position).normalized;
            Vector3 targetPosition = enemy.transform.position + directionAwayFromPlayer * enemy.minimumRange;
            enemy.Agent.SetDestination(targetPosition);
            }
            else
            {
            // Stop moving when within shooting range
            enemy.Agent.SetDestination(enemy.transform.position);
                // Shoot at the player only when within shooting range
                if (shotTimer > enemy.fireRate)
                {
                    Shoot();
                }
            }
        }
    else
    {
        losePlayerTimer += Time.deltaTime;

        // If the player has been out of sight for too long, change to SearchState
        if (losePlayerTimer > enemy.timeToSearch)
        {
            // Switch to SearchState using the live player's position (playerTransform)
            statemachine.ChangeState(new SearchState(statemachine.playerTransform));
        }
    }
    }
}
