using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

public class ThirdPersonMovement : MonoBehaviour
{
    [SerializeField] Transform _camera;
    [SerializeField] float _turnSmoothTime = 0.1f;
    float _turnSmoothVelocity;

    private void RotateInput(float horizontal, float vertical)
    {
        Vector3 dir = new Vector3(horizontal, 0, vertical).normalized;
        //Debug.Log(horizontal +" | "+ vertical);
        if(dir.magnitude >= 0.1f)
        {
            float targetAngle = Mathf.Atan2(dir.x, dir.z) * Mathf.Rad2Deg + _camera.eulerAngles.y;
            float angle = Mathf.SmoothDampAngle(transform.eulerAngles.y, targetAngle, ref _turnSmoothVelocity, _turnSmoothTime);
            this.transform.rotation = Quaternion.Euler(0f, angle, 0f);
        }
    }

    private void LookAt(Transform target)
    {
        var newRotation = Quaternion.LookRotation(target.position - transform.position, transform.forward);
        newRotation.x = 0f;
        newRotation.z = 0f;
        transform.rotation = Quaternion.Slerp(transform.rotation, newRotation, Time.deltaTime);
    }


}
