using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class AudioEntity : MonoBehaviour
{
    GameObject _trackedObject;
    bool _isTracking = false;

    bool _isLooping = false;
    float _length = 0;
    float _time = 0;
    bool _isPlaying = false;
    AudioSource _source;

    private void Awake()
    {
        _source = GetComponent<AudioSource>();
    }

    private void Update()
    {
        if (!_isLooping && _time > _length)
        {
            Destroy(this.gameObject);
        }
        _time += Time.deltaTime;

        if (_isTracking)
            this.transform.position = _trackedObject.transform.position;
    }

    public void PlayClip(AudioClip clip, bool isLooping = false)
    {
        _source.clip = clip;
        _source.Play();
        _isLooping = isLooping;
    }

    public void Track(GameObject go)
    {
        _trackedObject = go;
        _isTracking = true;
    }
}
