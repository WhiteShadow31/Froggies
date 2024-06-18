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

    [Header("Scene Ambiance")]
    public string ambianceName = "AMB_Foret";

    AudioEntity _saveAudio;

    private void Awake()
    {
        _instance = this;

        Object[] ambiants = Resources.LoadAll("Audio/Clips/Ambiant", typeof(AudioClip));
        Object[] enigmes = Resources.LoadAll("Audio/Clips/Enigmes", typeof(AudioClip));
        Object[] frogs = Resources.LoadAll("Audio/Clips/Frog", typeof(AudioClip));
        Object[] uis = Resources.LoadAll("Audio/Clips/UI", typeof(AudioClip));

        LoadToClips(ambiants);
        LoadToClips(enigmes);
        LoadToClips(frogs);
        LoadToClips(uis);
    }

    void LoadToClips(object[] objs)
    {
        for(int i = 0; i < objs.Length; i++)
        {
            clips.Add((AudioClip)objs[i]);
        }
    }

    private void Start()
    {
        PlayAmbient(ambianceName);
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
            audio.SetBaseVolume(volume, pitch);
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
            audio.Volume(GeneralVolume * value * volume, pitch);

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

    public void PlaySaveAt(Vector3 position)
    {
            if(_saveAudio != null)
                Destroy(_saveAudio.gameObject);
            
            _saveAudio = PlayClipAt(position, "UI_Save",false, 1f, 1f, Camera.main.gameObject);
    }
    public void StopSave()
    {
        if(_saveAudio != null)
                Destroy(_saveAudio.gameObject);
    }

    public void PlayAmbient(string ambientName)
    {
        for(int i = 0; i < _musics.Count; i++)
        {
            if(_musics[i] != null)
            {
                Destroy(_musics[i].gameObject);
            }
        }

        PlayClipAt(Vector3.zero, ambientName, true, 1f, 1, Camera.main.gameObject);
    }
}