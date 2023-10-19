using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StateMachine : IStateMachine
{
    protected IState _actualState;
    protected Transform _transform;

    public StateMachine(Transform trform) { _transform = trform; }

    public virtual void InitStateMachine()
    {

    }

    public virtual void Start()
    {
        InitStateMachine();

        if (_actualState != null)
            _actualState.Start();
    }
    public virtual void Update(float time)
    {
        if (_actualState != null)
            _actualState.Update(time);
    }
    public virtual void FixedUpdate(float time)
    {
        if (_actualState != null)
            _actualState.FixedUpdate(time);
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
        return _transform;
    }
}

public interface IStateMachine
{
    public void InitStateMachine();
    public void Start();
    public void Update(float time);
    public void FixedUpdate(float time);
    public void Exit(IState newState);
    public Transform GetTransform();
}
