using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuManager : MonoBehaviour
{
    public static MenuManager Instance;

    public Menu actualMenu;
    protected Menu m_starterMenu;

    // MENU 
    protected bool m_isInMenu = false;
    public bool IsInMenu {  get { return m_isInMenu; } }


    private void Awake()
    {
        Instance = this;

        if(actualMenu != null)
            m_starterMenu = actualMenu;
    }

    public void TryToOpenMenu()
    {
        if (m_isInMenu)
        {
            CloseMenu();
        }
        else
        {
            OpenMenu();
        }
    }


    protected void OpenMenu()
    {
        m_isInMenu = true;
        actualMenu.OpenMenu();
    }
    protected void CloseMenu()
    {
        m_isInMenu = false;
        actualMenu.CloseMenu();
    }

    public void PressSelectedButton()
    {
        if(actualMenu != null)
            actualMenu.PressSelectedButton();
    }

    public void ChangeSelectedButton(Vector2 dir)
    {
        if (actualMenu != null)
            actualMenu.ChangeSelectedButton(dir);
    }

    public void CloseGame()
    {
        if (m_isInMenu)
        {
            Application.Quit();
        }
    }
    public void StartNewGame()
    {
        if(SceneManager.GetActiveScene().buildIndex + 1 < SceneManager.sceneCount)
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
        else
            SceneManager.LoadScene(0);
        
    }
    public void LoadGame()
    {

    }
}
