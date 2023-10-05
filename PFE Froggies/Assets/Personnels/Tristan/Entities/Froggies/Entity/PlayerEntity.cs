using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class PlayerEntity : LivingEntity
{
    bool _forwardInput = false;
    bool _jumpInput = false;
    float _horizontalInput = 0;
    float _verticalInput = 0;


    protected StateMachinePlayer _smPlayer;

    protected override void Start()
    {
        base.Start();
        _smPlayer = new StateMachinePlayer(this);
        _smPlayer.Start();
    }


    // Update is called once per frame
    protected override void Update()
    {
        base.Update();

        _smPlayer.Update(Time.deltaTime);


        _jumpInput = Input.GetKey(KeyCode.Space);
        _horizontalInput = Input.GetAxisRaw("Horizontal");
        _verticalInput = Input.GetAxisRaw("Vertical");
        _forwardInput = Input.GetKey(KeyCode.Q);
    }
    
    protected override void FixedUpdate()
    {
        base.FixedUpdate();

        _smPlayer.FixedUpdate(Time.fixedDeltaTime);

        if (_jumpInput)
            Jump();

        if (_forwardInput)
            Move();

        Rotate(_horizontalInput, _verticalInput);
        
    }

    protected virtual void TongueHit()
    {

    }
}

public interface IFrogEntity
{
    public void TongueHit();
}
