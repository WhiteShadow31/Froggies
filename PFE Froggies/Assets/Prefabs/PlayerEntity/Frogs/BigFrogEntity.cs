using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BigFrogEntity : PlayerEntity
{
    [SerializeField] protected bool _push = true;
    protected override void TongueHitObject(Vector3 target)
    {
        Collider[] cols = Physics.OverlapSphere(target, _tongueHitRadius, _tongueHitLayerMask);
        foreach (Collider col in cols)
        {
            if (col.transform.TryGetComponent<IIntaractableEntity>(out IIntaractableEntity entity))
            {
                int sign = 1;
                if (_push)
                    sign = 1;
                else
                    sign = -1;

                    entity.Push(sign * (col.transform.position - _tongueEndTransform.position).normalized, _tongueHitForce);
            }
        }
    }
}
