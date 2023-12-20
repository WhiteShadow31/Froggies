using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StatePlayerUseTongue : StatePlayer
{
    bool _isGrounded = false;
    Vector3 _targetPosition = Vector3.zero;

    public StatePlayerUseTongue(StateMachinePlayer sm) : base(sm) { }

    public override void Start()
    {
        //_smPlayer.entity.ShowTongueAim(_targetPosition = _smPlayer.entity.TongueAimPosition());
    }
    public override void Update(float time)
    {
        _isGrounded = _smPlayer.entity.IsGrounded;
        _targetPosition = _smPlayer.entity.TongueAimPosition(); // USE A RAYCAST TO FIND POSITION TO AIM

        if (_smPlayer.entity.EndTongueAimInput)
        {

            // START TONGUE USE
        }
    }
    public override void FixedUpdate(float time)
    {
        if (_isGrounded)
        {
            _smPlayer.entity.Rotate();

        }
    }
    public override void Exit()
    {

    }
}
