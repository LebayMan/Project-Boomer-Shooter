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
public void ShootBoss()
{
    Debug.Log("SHOOT");
    Transform gunbarrel = enemy.gunBarrel;

    // Instantiate the bullet
    GameObject bullet = GameObject.Instantiate(Resources.Load("Prefabs/Bullet1") as GameObject, gunbarrel.position, enemy.transform.rotation);

    // Adjust the bullet's rotation to be rotated 90 degrees
    bullet.transform.rotation = Quaternion.Euler(bullet.transform.eulerAngles.x + 90, bullet.transform.eulerAngles.y, bullet.transform.eulerAngles.z);

    // Calculate the shooting direction
    Vector3 shootDirection = (enemy.Player.transform.position - gunbarrel.transform.position).normalized;

    // Add a slight random deviation for more realistic shooting
    shootDirection = Quaternion.Euler(Random.Range(-1f, 1f), Random.Range(-1f, 1f), 0) * shootDirection;

    // Apply velocity to the bullet
    Rigidbody bulletRigidbody = bullet.GetComponent<Rigidbody>();
    bulletRigidbody.velocity = shootDirection * enemyBoss.bulletSpeed;

    shotTimer = 0;
}

    public override void Perform()
    {
        if (enemy.CanSeePlayer())
        {
            losePlayerTimer = 0;
            moveTimer += Time.deltaTime;
            shotTimer += Time.deltaTime;
            enemy.transform.LookAt(enemy.Player.transform);
            if(shotTimer > enemy.fireRate)
            {
                Debug.Log("SHOOT1");
                Shoot();
            }
            if(moveTimer > Random.Range(3,7))
            {
                enemy.Agent.SetDestination(enemy.transform.position * (Random.insideUnitCircle * 5));

                moveTimer = 0;
                
            }
        }
    
        else
        {
            losePlayerTimer += Time.deltaTime;
            if(losePlayerTimer > 8)
            {
                statemachine.ChangeState(new PatrolState());
            }
        }
    }
}
