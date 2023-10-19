using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StateMachinePlayer : StateMachine
{
    public PlayerEntity entity;

    // STATES
    public StatePlayerIdle idle;
    public StatePlayerOnFrog onFrog;
    public StatePlayerJump jump;

    public StateMachinePlayer(PlayerEntity plr) : base(plr.transform) { entity = plr; }

    public override void InitStateMachine()
    {
        idle = new StatePlayerIdle(this);
        onFrog = new StatePlayerOnFrog(this);
        jump = new StatePlayerJump(this);

        _actualState = idle;        
    }
}
