using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

[Serializable]
public class GroundedController
{
    [SerializeField] Transform _groundCheck;
    [SerializeField] float _groundRadius;
    [SerializeField] LayerMask _groundMask;
    public bool IsGrounded { get { return Physics.OverlapSphere(_groundCheck.position, _groundRadius, _groundMask).Length > 0; } }

    public GroundedController() { }
    public GroundedController(Transform groundCheck, float groundRadius, LayerMask groundMask) 
    { 
        _groundCheck = groundCheck;
        _groundRadius = groundRadius;
        _groundMask = groundMask;
    }
}
