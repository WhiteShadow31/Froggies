using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using UltimateAttributesPack;

public class UIManager : MonoBehaviour
{
    [SerializeField] GameObject _pauseMenuObject;
    [SerializeField] GameObject _pauseMenuFirstSelected;
    [Space]
    [SerializeField, Scene] string _mainMenuSceneName;

    bool _inPauseMenu;
    public bool InPauseMenu => _inPauseMenu;

    void SetFirstSelected(GameObject obj)
    {
        EventSystem.current.SetSelectedGameObject(null);
        EventSystem.current.SetSelectedGameObject(obj);
    }

    public void SetPauseMenu(bool state)
    {
        if (SceneManager.GetActiveScene().name == _mainMenuSceneName)
            return;

        _inPauseMenu = state;
        _pauseMenuObject.SetActive(state);

        if(state)
            SetFirstSelected(_pauseMenuFirstSelected);
    }





}