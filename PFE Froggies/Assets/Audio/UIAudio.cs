using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class UIAudio : MonoBehaviour
{
    GameObject _lastSelected = null;
    void Update()
    {
        if(_lastSelected != null)
        {
            if(EventSystem.current.currentSelectedGameObject != _lastSelected)
            {
                if(AudioGenerator.Instance != null)
                    AudioGenerator.Instance.PlayClipAt(Camera.main.transform.position, "UI_Select_01");
            }
        }
        
        _lastSelected = EventSystem.current.currentSelectedGameObject;
    }

    public void ClickOnButton()
    {
        if(AudioGenerator.Instance != null)
            AudioGenerator.Instance.PlayClipAt(Camera.main.transform.position, "UI_Valide_01");
    }

    public void Back()
    {
        if(AudioGenerator.Instance != null)
            AudioGenerator.Instance.PlayClipAt(Camera.main.transform.position, "UI_Back");
    }
}
