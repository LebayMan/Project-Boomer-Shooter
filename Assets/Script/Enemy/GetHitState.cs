using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GetHitState : BaseState
{
    private float recoilDuration = 50f; // Time the enemy spends recoiling
    private float recoilTimeElapsed = 5f;
    private Vector3 recoilDirection;
    private float recoilDistance;

    public GetHitState(Enemy enemy, float damage)
    {
        this.enemy = enemy;
        this.recoilDirection = -enemy.transform.forward; // Recoil in the opposite direction
        this.recoilDistance = damage * 0.1f; // Scale the recoil distance based on damage
    }

    public override void Enter()
    {
        recoilTimeElapsed = 0f;
        Debug.Log("Entered GetHitState: Applying recoil.");
    }

    public override void Perform()
    {
        // Apply recoil movement
        if (recoilTimeElapsed < recoilDuration)
        {
            float recoilSpeed = recoilDistance / recoilDuration;
            enemy.transform.position += recoilDirection * recoilSpeed * Time.deltaTime;
            recoilTimeElapsed += Time.deltaTime;
        }
        else
        {
            statemachine.ChangeState(new AttackState());
        }
    }

    public override void Exit()
    {
        Debug.Log("Exiting GetHitState: Recoil finished.");
    }
}

