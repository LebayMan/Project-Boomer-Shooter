using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SearchState : BaseState
{
    private Transform playerTransform;

    // Pass the player's live transform to the state
    public SearchState(Transform player)
    {
        playerTransform = player;
    }

    public override void Enter()
    {
        // No need to set a static destination since we are following the player's live position
    }

    public override void Perform()
    {
        // Continuously follow the player's current position
        enemy.Agent.SetDestination(playerTransform.position);

        // Check the distance to the player
        float distanceToPlayer = Vector3.Distance(enemy.transform.position, playerTransform.position);
        // Get the direction the enemy is moving in
        Vector3 moveDirection = enemy.Agent.velocity.normalized;

        // Rotate the enemy to face the movement direction
        if (moveDirection != Vector3.zero)
        {
            Quaternion lookRotation = Quaternion.LookRotation(moveDirection);
            enemy.transform.rotation = Quaternion.Slerp(enemy.transform.rotation, lookRotation, Time.deltaTime * enemy.turnSpeed);
        }
        // If the enemy sees the player again, change to AttackState
        if (enemy.CanSeePlayer())
        {
            statemachine.ChangeState(new AttackState());
        }
    }

    public override void Exit()
    {
        // Reset any necessary variables when leaving the state
    }
}
