using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using UltimateAttributesPack;
using UnityEngine.InputSystem;

public class UIManager : MonoBehaviour
{
    [SerializeField] GameObject _pauseMenuObject;
    [SerializeField] GameObject _pauseMenuFirstSelected;
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
        Time.timeScale = state ? 0f : 1f;
        if(state)
            SetFirstSelected(_pauseMenuFirstSelected);
    }

    public void QuitToMainMenu()
    {
        SceneManager.LoadScene(_mainMenuSceneIndex);
    }

    public void QuitGame()
    {
        Application.Quit();
    }

}