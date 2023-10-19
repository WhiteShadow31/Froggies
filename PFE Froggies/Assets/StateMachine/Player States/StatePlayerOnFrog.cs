using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StatePlayerOnFrog : State
{
    protected StateMachinePlayer _smPlayer;

    // Constructor
    public StatePlayerOnFrog(StateMachinePlayer sm) : base(sm) { _smPlayer = sm; }

    public override void Start()
    {

    }
    public override void Update(float time)
    {
        _smPlayer.entity.transform.position = _smPlayer.entity.GetMountTransform.position;


        if (_smPlayer.entity.JumpInput)
        {
            //_smPlayer.entity.StopMount();
            _smPlayer.entity.Jump();
        }
        

        if (_smPlayer.entity.MountInput)
        {
            //_smPlayer.entity.StopMount();
        }
    }
    public override void FixedUpdate(float time)
    {
            _smPlayer.entity.Rotate();
    }
    public override void Exit()
    {

    }
}
