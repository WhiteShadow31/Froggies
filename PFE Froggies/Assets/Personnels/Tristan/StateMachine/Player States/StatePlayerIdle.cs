using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class StatePlayerIdle : State
{
    protected StateMachinePlayer _smPlayer;

    // Constructor
    public StatePlayerIdle(StateMachinePlayer sm) : base(sm) { _smPlayer = sm; }

    public override void Start()
    {
        Debug.Log("IDLE");
    }
    public override void Update(float time)
    {

    }
    public override void FixedUpdate(float time)
    {

    }
    public override void Exit()
    {

    }
}
