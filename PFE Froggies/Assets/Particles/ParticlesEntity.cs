using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParticlesEntity : MonoBehaviour
{
    GameObject _trackedObject;
    bool _isTracking = false;
    ParticleSystem _particleSystem;

    private void Awake()
    {
        _particleSystem = GetComponent<ParticleSystem>();
    }

    private void Update()
    {
        if(_particleSystem != null && !_particleSystem.isPlaying)
        {
            Destroy(this.gameObject);
        }

        if(_isTracking)
            this.transform.position = _trackedObject.transform.position;
    }

    public void Track(GameObject go)
    {
        _trackedObject = go;
        _isTracking = true;
    }
}
