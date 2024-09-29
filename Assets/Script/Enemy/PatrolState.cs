using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PatrolState : BaseState
{
    public int waypointIndex;
    
    public override void Enter()
    {
        
    }
        public override void Perform()
    {
        PatrolCyle();
        if(enemy.CanSeePlayer())
        {
            statemachine.ChangeState(new AttackState());
        }
    }
        public override void Exit()
    {
        
    }

    public void PatrolCyle()
    {
        if(enemy.Agent.remainingDistance < 0.2f)
        {
            enemy.waitTimer += Time.deltaTime;
            if(enemy.waitTimer > 3)
            {
            if(waypointIndex < enemy.path.waypoints.Count -1)
                waypointIndex++;
            else
                waypointIndex = 0;
            enemy.Agent.SetDestination(enemy.path.waypoints[waypointIndex].position);
            }
        }
    }

}
