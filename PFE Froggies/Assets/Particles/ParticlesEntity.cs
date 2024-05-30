using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParticlesEntity : MonoBehaviour
{
    ParticleSystem _particleSystem;

    private void Awake()
    {
        _particleSystem = GetComponent<ParticleSystem>();
    }

    private void Update()
    {
        Debug.Log(_particleSystem.isPlaying);
        if(_particleSystem != null && !_particleSystem.isPlaying)
        {
            Destroy(this.gameObject);
        }
    }
}
