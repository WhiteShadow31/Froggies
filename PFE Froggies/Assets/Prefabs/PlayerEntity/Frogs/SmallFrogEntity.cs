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
            if (cols[index].transform.TryGetComponent<IIntaractableEntity>(out IIntaractableEntity entity))
            {
                // PUSH ENTITY
                entity.Push((cols[index].transform.position - this.transform.position).normalized, _tongueHitForce);
            }
            else if(cols[index].transform.TryGetComponent<SmallFrogEntity>(out SmallFrogEntity frog))
            {
                // PUSH PLAYER
                frog.PushPlayer((cols[index].transform.position - this.transform.position).normalized, _tongueHitForce);
            }
            else
                _rigidbodyController.AddForce(this.transform.forward, _tongueHitForce * 1.5f, ForceMode.Impulse);
        }
    }
}
