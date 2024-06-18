using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioEntity : MonoBehaviour
{
    public float volume = 1;
    public float pitch = 1;
    GameObject _trackedObject;
    bool _isTracking = false;

    bool _isLooping = false;
    float _length = 0;
    float _time = 0;
    bool _isPlaying = false;
    AudioSource _source;

    private void Awake()
    {
        _source = this.gameObject.AddComponent<AudioSource>();
    }

    private void Update()
    {
        if(_isPlaying)
        {
            if (!_isLooping && _time > _length)
            {
                if(AudioGenerator.Instance != null)
                    AudioGenerator.Instance.RemoveAudioEntity(this);

                Destroy(this.gameObject);
            }
            _time += Time.deltaTime;

            if (_isTracking)
                this.transform.position = _trackedObject.transform.position;
        }
    }

    public void Play(AudioClip clip, bool isLooping = false)
    {
        _source.clip = clip;
        _length = clip.length;
        _isLooping = isLooping;
        
        _source.loop = _isLooping;
        //_source.spatialBlend = 1;
        _source.Play();

        _isPlaying = true;
    }

    public void SetBaseVolume(float volume = 1f, float pitch = 1f)
    {
        this.volume = volume;
        this.pitch = pitch;
    }

    public void Volume(float volume = 1f, float pitch = 1f)
    {
        _source.volume = volume;
        _source.pitch = pitch;
    }

    public void Follow(GameObject go)
    {
        _trackedObject = go;
        _isTracking = true;
    }
}
