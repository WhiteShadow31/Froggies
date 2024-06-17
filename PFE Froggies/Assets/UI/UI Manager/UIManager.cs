using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using UltimateAttributesPack;
using UnityEngine.InputSystem;
using System.Collections.Generic;

public class UIManager : MonoBehaviour
{
    [SerializeField] GameObject _pauseMenuObject;
    [SerializeField] GameObject _pauseMenuFirstSelected;
    [Space]
    [SerializeField] GameObject _optionsMenuObject;
    [SerializeField] GameObject _optionsMenuFirstSelected;
    [Space]
    [SerializeField, Scene] int _mainMenuSceneIndex;

    bool _inPauseMenu;
    public bool InPauseMenu => _inPauseMenu;

    void SetFirstSelected(GameObject obj)
    {
        EventSystem.current.SetSelectedGameObject(null);
        EventSystem.current.SetSelectedGameObject(obj);
    }

    public void SetPauseMenu(bool state)
    {
        _inPauseMenu = state;
        _pauseMenuObject.SetActive(state);
        if(state)
            SetFirstSelected(_pauseMenuFirstSelected);
    }

    public void Quit()
    {
        if(SceneManager.GetActiveScene().buildIndex == _mainMenuSceneIndex)
            Application.Quit();
        else
        {
            // Save de tout
            Saver.SaveActiveScene(Saver.saveIndex);

            SceneManager.LoadScene(_mainMenuSceneIndex);
        }
    }

    public void SetOptionsMenu(bool state)
    {
        _optionsMenuObject.SetActive(state);
        if (state)
            SetFirstSelected(_optionsMenuFirstSelected);
    }

    public void SetPause(bool state)
    {
        Time.timeScale = state ? 0f : 1f;
    }
}