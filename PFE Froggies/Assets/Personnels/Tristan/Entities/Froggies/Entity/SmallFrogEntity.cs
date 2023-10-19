using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SmallFrogEntity : PlayerEntity
{
    protected override void TongueHitObject(Vector3 target)
    {
        Collider[] cols = Physics.OverlapSphere(target, _tongueHitRadius, _tongueHitLayerMask);
        float minDist = 1000;
        int index = -1;
        for(int i = 0; i< cols.Length; i++)
        {
            float dist = Vector3.Distance(cols[i].transform.position, target);
            if (Vector3.Distance(cols[i].transform.position, target) < minDist)
            {
                minDist = dist;
                index = i;
            }
        }
        if(index >= 0)
        {
            _rigidbodyController.AddForce(this.transform.forward, _tongueHitForce, ForceMode.Impulse);
        }
    }
}
