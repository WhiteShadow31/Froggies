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
        Debug.Log(_smPlayer.entity.gameObject.name + " : " + "ONFROG");
    }
    public override void Update(float time)
    {
        _smPlayer.entity.transform.position = _smPlayer.entity.GetMountTransform.position;


        if (_smPlayer.entity.JumpPressInput)
        {
            _smPlayer.Exit(_smPlayer.jump);
        }
        

        if (_smPlayer.entity.MountInput)
        {
            _smPlayer.Exit(_smPlayer.idle);
        }
    }
    public override void FixedUpdate(float time)
    {
            _smPlayer.entity.Rotate();
    }
    public override void Exit()
    {
        _smPlayer.entity.StopMount();
    }
}
