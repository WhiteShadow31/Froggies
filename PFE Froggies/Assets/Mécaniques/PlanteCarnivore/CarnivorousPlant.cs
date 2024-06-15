using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

[RequireComponent(typeof(BoxCollider))]
public class CarnivorousPlant : MonoBehaviour
{
    [SerializeField] float _detectionRadius = 5f;
    [SerializeField] LayerMask _detectionMask;

    [SerializeField] float _timeToHit = 0.3f;
    [SerializeField] Transform _plantHead;
    [SerializeField] Transform _respawnPoint;
    Vector3 _plantHeadStartPos;

    Transform _targetPos;
    bool _tryToHit = false;
    [Space]
    [SerializeField] LayerMask _stunMask;
    bool _isStunned = false;
    [SerializeField] float _stunTimeMAX = 1.2f;
    float _stunTime = 0;

    private void Awake()
    {
        _plantHeadStartPos = _plantHead.position;
    }

    private void FixedUpdate()
    {
        // STUNNED
        if (!_isStunned)
        {
            // Not trying to hit something
            if ( !_tryToHit)
            {
                PlayerEntity entity = DetectTarget();
                if (entity)
                {
                    _targetPos = entity.transform;
                    StartCoroutine(CoroutineHeadHit(_timeToHit));
                }

            }
            // Is trying to hit something
            else if (_targetPos != null) 
            {
                Vector3 posToCheck = _targetPos.position;
                posToCheck.y = this.transform.position.y;
                float newDist = Vector3.Distance(this.transform.position, posToCheck);

                if (newDist-0.5f > _detectionRadius)
                {
                    _tryToHit = false;    
                }
            }
            
        }
        // NOT STUNNED
        else
        {
            _plantHead.position = _plantHeadStartPos;
            _targetPos = null;

            _tryToHit = false;

            if(_stunTime < _stunTimeMAX)
            {
                _stunTime += Time.fixedDeltaTime;
            }
            else
            {
                _stunTime = 0;
                _isStunned = false;
            }
        }
    }

    protected PlayerEntity DetectTarget()
    {
        Collider[] cols = Physics.OverlapSphere(this.transform.position, _detectionRadius, _detectionMask);

        List<PlayerEntity> targets = new List<PlayerEntity>();
        foreach (Collider col in cols)
        {
            if(col.transform.TryGetComponent<PlayerEntity>(out PlayerEntity target))
                targets.Add(target);
        }

        if(targets.Count == 1)
        {
            return targets[0];
        }
        else if(targets.Count > 1) 
        {
            float dist = 1000f;
            PlayerEntity entity = null;

            foreach (PlayerEntity target in targets)
            {
                float newDist = Vector3.Distance(_plantHeadStartPos, target.transform.position);
                if (newDist < dist)
                {
                    dist = newDist;
                    entity = target;
                }
            }
            return entity;
        }
        return null;

    }

    IEnumerator CoroutineHeadHit(float duration)
    {
        _tryToHit = true;

        float time = 0;
        Vector3 startPosition = _plantHead.position;
        while ((time < duration) && _tryToHit)
        {
            if (_tryToHit)
            {
                _plantHead.position = Vector3.Lerp(startPosition, _targetPos.position, time / duration);
                time += Time.deltaTime;
                yield return null;
            }
            else
            {
                break;
            }
        }

        if (_tryToHit)
        {
            if(AudioGenerator.Instance != null)
                AudioGenerator.Instance.PlayClipAt(this.transform.position, "ENGM_Plante_Croc_01");
            _targetPos.GetComponent<PlayerEntity>().Respawn(_respawnPoint.position);
        }

        _plantHead.position = _plantHeadStartPos;
        _targetPos = null;

        _tryToHit = false;
    }

    private void OnCollisionEnter(Collision collision)
    {
        if ((_stunMask.value & (1 << collision.transform.gameObject.layer)) > 0)
        {
            _stunTime = 0;
            _isStunned = true;

            if(AudioGenerator.Instance != null)
                AudioGenerator.Instance.PlayClipAt(this.transform.position, "ENGM_Plante_Stun_01");
        }
    }

    private void OnDrawGizmos()
    {
        Color col = Color.red;
        col.a = 0.3f;
        Gizmos.color = col;
        Gizmos.DrawSphere(this.transform.position, _detectionRadius);
    }
}
