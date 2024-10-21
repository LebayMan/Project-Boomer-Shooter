using UnityEngine;

public class StunState : BaseState
{
    private float stunDuration;
    private float recoilDistance;
    private Vector3 recoilDirection;
    private Transform playerTransform;

    private Vector3 initialPosition;
    private Vector3 targetPosition;
    private float recoilTimeElapsed;
    private float recoilDuration = 0.2f; // Adjust this for how quickly the recoil happens

    public StunState(Enemy enemy, float stunDuration, float damage, Transform player) : base()
    {
        this.enemy = enemy;
        this.stunDuration = stunDuration;
        this.recoilDistance = damage * 0.014f;
        this.playerTransform = player;
    }

    public override void Enter()
    {
        enemy.StuntTime = 0;
        enemy.Stunt = true;
        Debug.Log("Enemy is stunned!");

        if (enemy.agent != null)
        {
            enemy.agent.isStopped = true; // Stop movement during stun
        }

        // Calculate recoil direction (opposite to the player or source of damage)
        if (playerTransform != null)
        {
            recoilDirection = (enemy.transform.position - playerTransform.position).normalized;
        }
        else
        {
            // Default recoil direction if no playerTransform is available
            recoilDirection = enemy.transform.forward * -1; // Just move backward
        }

        // Calculate initial and target positions for smooth recoil
        if (enemy.gameObject1 != null)
        {
            initialPosition = enemy.gameObject1.transform.position;
            targetPosition = initialPosition + recoilDirection * recoilDistance;
        }

        recoilTimeElapsed = 0; // Reset recoil timer
    }

    public void Update()
    {
        // Stun timer logic happens in Perform, so nothing needed here for now
    }

    public override void Perform()
    {
        // Smoothly apply recoil movement over time
        if (enemy.gameObject1 != null && recoilTimeElapsed < recoilDuration)
        {
            recoilTimeElapsed += Time.deltaTime;
            float t = recoilTimeElapsed / recoilDuration;
            enemy.gameObject1.transform.position = Vector3.Lerp(initialPosition, targetPosition, t); // Smooth transition
        }

        // Increment the stun timer
        enemy.StuntTime += Time.deltaTime;

        // Check if stun duration is over
        if (enemy.StuntTime >= stunDuration)
        {
            enemy.Stunt = false;
            Debug.Log("Stun finished.");
            statemachine.ChangeState(new SearchState(statemachine.playerTransform));
            if (enemy.agent != null)
            {
                enemy.agent.isStopped = false; // Resume movement
            }
        }
    }

    public override void Exit()
    {
        Debug.Log("Exiting stun state.");
    }
}
