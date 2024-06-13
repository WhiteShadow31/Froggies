using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AudioGenerator : MonoBehaviour
{
    public Slider generalAudio;
    public Slider sfxAudio;
    public Slider musicAudio;
    public float GeneralVolume => generalAudio.value / 4;
    public float SfxVolume => sfxAudio.value / 4;
    public float MusicVolume => musicAudio.value / 4;

    public List<AudioClip> clips = new List<AudioClip>();
    static AudioGenerator _instance;
    public static AudioGenerator Instance => _instance;

    List<AudioEntity> _sfxs = new List<AudioEntity>();
    List<AudioEntity> _musics = new List<AudioEntity>();

    private void Awake()
    {
        _instance = this;
    }

    private void Start()
    {
        PlayClipAt(Vector3.zero, "AMB_Foret", true, 1, 1, Camera.main.gameObject);
    }

    private void LateUpdate()
    {
        for(int i = 0; i < _sfxs.Count; i++)
        {
            if(_sfxs[i] != null)
                _sfxs[i].Volume(GeneralVolume * SfxVolume * _sfxs[i].volume, _sfxs[i].pitch);
        }

        for(int j = 0; j < _musics.Count; j++)
        {
            if(_musics[j] != null)
            {
                Debug.Log(GeneralVolume + " / "+MusicVolume + " / "+ _musics[j].volume);
                _musics[j].Volume(GeneralVolume * MusicVolume * _musics[j].volume, _musics[j].pitch);
            }
        }
    }

    AudioClip GetClipByName(string name)
    {
        for(int i = 0; i < clips.Count; i++)
        {
            if(clips[i].name == name)
                return clips[i];
        }
        return null;
    }

    public AudioEntity PlayClip(string name, bool isLooping = false, float volume = 1, float pitch = 1, GameObject tracked = null)
    {
        GameObject go = new GameObject("Audio SFX_"+name);
        AudioEntity audio = go.AddComponent<AudioEntity>();

        AudioClip clip = GetClipByName("SFX_"+name);
        if(clip != null)
        {
            audio.Play(clip, isLooping);
            float value = 1;
            if(name.Length > 3)
            {
                string nomenclature = name.Substring(0, 3);
                switch(nomenclature)
                {
                    case "AMB":
                        value = MusicVolume;
                        _musics.Add(audio);
                        break;
                    case "ENG":
                        value = SfxVolume;
                        _sfxs.Add(audio);
                        break;
                    case "GRE":
                        value = SfxVolume;
                        _sfxs.Add(audio);
                        break;
                    case "UI_":
                        value = SfxVolume;
                        _sfxs.Add(audio);
                        break;
                }
            }
            audio.SetBaseVolume(volume, pitch);
            //audio.Volume(GeneralVolume * value * volume, pitch);

            if(tracked != null)
                audio.Follow(tracked);
        }
        return audio;
    }
    public AudioEntity PlayClipAt(Vector3 position, string name, bool isLooping = false, float volume = 1, float pitch = 1, GameObject tracked = null)
    {
        AudioEntity audio = PlayClip(name, isLooping, volume, pitch, tracked);
        audio.transform.position = position;

        return audio;
    }

    public void RemoveAudioEntity(AudioEntity audio)
    {
        _sfxs.Remove(audio);
        _musics.Remove(audio);
    }
}
