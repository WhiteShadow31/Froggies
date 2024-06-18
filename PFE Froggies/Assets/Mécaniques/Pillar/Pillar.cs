using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[RequireComponent(typeof(BoxCollider))]
public class Pillar : MonoBehaviour
{
    public List<Pillar> modifiedPillar;
    public float timeToRise = 1f;
    float _timerRising = 0f;
    public LayerMask detectMask;
    public bool lower = true;

    

    public float offsetHeight = -1;
    float _startingHeight = 0;
    bool moving = false;

    public ParticleSystem movingParticles;

    private void Awake()
    {
        _startingHeight = this.transform.position.y;
    }

    private void FixedUpdate()
    {
        float height = 0.1f;

        Vector3 colSize = this.GetComponent<BoxCollider>().size;
        
        Vector3 center = this.transform.position;
        center.y += colSize.y;
        
        colSize.y = height;


        Collider[] cols = Physics.OverlapBox(center, colSize / 2, Quaternion.identity, detectMask);

        // Something is on the pillar
        if (cols.Length > 0)
        {
            foreach(Pillar pil in modifiedPillar)
            {
                pil.Move(Time.fixedDeltaTime);
            }
        }
        else
        {
            foreach (Pillar pil in modifiedPillar)
            {
                pil.StopMove();
            }
        }

        if (!moving)
        {
            if (_timerRising > 0)
            {
                _timerRising -= Time.fixedDeltaTime;

                Vector3 pos = this.transform.position;
                pos.y = Mathf.Lerp(_startingHeight, _startingHeight + offsetHeight, _timerRising / timeToRise);

                this.transform.position = pos;

                if (movingParticles != null)
                    movingParticles.Play();
            }
            else
            {
                _timerRising = 0;
                if (movingParticles != null)
                    movingParticles.Stop();
            }
        }
    }

    public void Move(float time)
    {
        moving = true;
        if (_timerRising < timeToRise)
        {
            _timerRising += time;

            Vector3 pos = this.transform.position;
            pos.y = Mathf.Lerp(_startingHeight, _startingHeight + offsetHeight, _timerRising / timeToRise);

            this.transform.position = pos;

            if (movingParticles != null)
                movingParticles.Play();
        }
        else
        {
            _timerRising = timeToRise;
            if (movingParticles != null)
                movingParticles.Stop();
        }
    }

    public void StopMove()
    {
        moving = false;
    }
}
