using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StateMachinePlayer : StateMachine
{
    [SerializeField] public PlayerEntity controller;

    // STATES
    public StatePlayerIdle idle;

    public StateMachinePlayer(PlayerEntity plr) { controller = plr; }

    public override void InitStateMachine()
    {
        idle = new StatePlayerIdle(this);

        _actualState = idle;
    }
}
