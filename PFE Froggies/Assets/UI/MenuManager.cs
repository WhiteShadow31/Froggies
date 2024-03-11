using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;

public class MenuManager : MonoBehaviour
{
    public static MenuManager Instance;

    public Menu mainMenu;
    public Menu openedMenu;
    public GameObject buttonSelected;

    private void Awake()
    {
        Instance = this;
    }

    public void OpenOrCloseMainMenu()
    {
        if(mainMenu != null && mainMenu.gameObject.activeInHierarchy)
        {
            mainMenu.gameObject.SetActive(!mainMenu.gameObject.activeInHierarchy);
        }
    }
}
