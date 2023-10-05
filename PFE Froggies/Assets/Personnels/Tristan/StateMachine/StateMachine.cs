using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StateMachine : MonoBehaviour, IStateMachine
{
    protected IState _actualState;

    public virtual void InitStateMachine()
    {

    }

    public virtual void Start()
    {
        InitStateMachine();

        if (_actualState != null)
            _actualState.Start();
    }
    public virtual void Update()
    {
        if (_actualState != null)
            _actualState.Update(Time.deltaTime);
    }
    public virtual void FixedUpdate()
    {
        if (_actualState != null)
            _actualState.FixedUpdate(Time.fixedDeltaTime);
    }
    public virtual void Exit(IState newState)
    {
        if (_actualState != null)
            _actualState.Exit();
        _actualState = newState;
        _actualState.Start();

    }

    public Transform GetTransform()
    {
        return this.transform;
    }
}

public interface IStateMachine
{
    public void InitStateMachine();
    public void Start();
    public void Update();
    public void FixedUpdate();
    public void Exit(IState newState);
    public Transform GetTransform();
}
