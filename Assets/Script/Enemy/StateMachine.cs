using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StateMachine : MonoBehaviour
{
    [Header("Reference")]
    public BaseState activestate;

    public void Initialise()
    {

        ChangeState(new PatrolState());
    }
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (activestate != null)
        {
            activestate.Perform();
        }
    }
    public void ChangeState(BaseState newstate)
    {
        if(activestate != null)
        {
            activestate.Exit();
        }
        activestate = newstate;

        if(activestate !=null)
        {
            activestate.statemachine = this;
            activestate.enemy = GetComponent<Enemy>();
            activestate.Enter();
        }
    }
}
