using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StatePlayer : State
{
    protected StateMachinePlayer _smPlayer;

    // Constructor
    public StatePlayer(StateMachinePlayer sm) : base(sm) { _smPlayer = sm; }

    public override void Start()
    {

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
