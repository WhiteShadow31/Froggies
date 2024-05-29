using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour
{
    public static SoundManager instance;

    [SerializeField] AudioSource _soundPrefab;
    [SerializeField] Transform _soundParent;

    private void Awake()
    {
        instance = this;
    }

    public void CreateSound(Sound sound, Transform targetTransform)
    {
        AudioSource audioSource = Instantiate(_soundPrefab, targetTransform.transform.position, Quaternion.identity, _soundParent);
        audioSource.gameObject.name = sound.Clip.name;
        audioSource.clip = sound.Clip;
        audioSource.volume = sound.Volume;
        audioSource.Play();
        float clipLenght = audioSource.clip.length;
        Destroy(audioSource.gameObject, clipLenght);
    }

    public void CreateRandomSound(Sound[] sounds, Transform targetTransform)
    {
        int randClipIndex = UnityEngine.Random.Range(0, sounds.Length);
        AudioSource audioSource = Instantiate(_soundPrefab, targetTransform.transform.position, Quaternion.identity, _soundParent);
        audioSource.gameObject.name = sounds[randClipIndex].Clip.name;
        audioSource.clip = sounds[randClipIndex].Clip;
        audioSource.volume = sounds[randClipIndex].Volume;
        audioSource.Play();       
        float clipLenght = audioSource.clip.length;
        Destroy(audioSource.gameObject, clipLenght);
    }
}

[Serializable]
public class Sound
{
    public AudioClip Clip;
    public float Volume;
}