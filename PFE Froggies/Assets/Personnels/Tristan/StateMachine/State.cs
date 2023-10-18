using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class State : IState
{
    // The state machine used for calling methods for state machine behaviors
    protected IStateMachine _stateMachine;


    // Constructor
    public State(IStateMachine sm) { _stateMachine = sm; }

    public virtual void Start()
    {

    }
    public virtual void Update(float time)
    {

    }
    public virtual void FixedUpdate(float time)
    {

    }
    public virtual void Exit()
    {

    }
}

public interface IState
{
    public void Start();
    public void Update(float time);
    public void FixedUpdate(float time);
    public void Exit();
}
