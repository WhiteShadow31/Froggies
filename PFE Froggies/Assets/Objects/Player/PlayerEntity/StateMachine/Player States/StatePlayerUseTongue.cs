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
        Debug.Log("TONGUE");
    }
    public override void Update(float time)
    {
        _isGrounded = _smPlayer.entity.IsGrounded;
        _targetPosition = _smPlayer.entity.TongueAimPosition(); // USE A RAYCAST TO FIND POSITION TO AIM

        if (_smPlayer.entity.EndTongueAimInput)
        {
            _smPlayer.entity.UseTongue();
            // START TONGUE USE
        }
    }
    public override void FixedUpdate(float time)
    {

    }
    public override void Exit()
    {

    }
}
