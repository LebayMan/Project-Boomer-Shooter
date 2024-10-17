using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StateMachine : MonoBehaviour
{
    [Header("Reference")]
    public BaseState activeState;
    public Transform playerTransform; // Reference to the player's Transform

    public void Initialise()
    {
        // Start the state machine with SearchState, tracking the player's live position
        ChangeState(new SearchState(playerTransform)); // Start with SearchState, passing the player's live position
    }

    void Update()
    {
        if (activeState != null)
        {
            activeState.Perform();
        }

        // Check if the current state is not AttackState, then go to SearchState
        if (!(activeState is AttackState))
        {
            ChangeState(new SearchState(playerTransform));
        }
    }

    public void ChangeState(BaseState newState)
    {
        if (activeState != null)
        {
            activeState.Exit();
        }

        activeState = newState;

        if (activeState != null)
        {
            activeState.statemachine = this;
            activeState.enemy = GetComponent<Enemy>();
            activeState.Enter();
        }
    }
}
