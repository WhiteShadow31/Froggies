using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuManager : MonoBehaviour
{
    public static MenuManager Instance;
    public GameObject menu;

    // MENU 
    protected bool m_isInMenu = false;
    public bool IsInMenu {  get { return m_isInMenu; } }

    // Button actually selected by the players
    [SerializeField] protected GamepadButton m_selectedButton;
    public GamepadButton SelectedButton { set { m_selectedButton = value; } }

    private void Awake()
    {
        Instance = this;

        if(menu)
            menu.SetActive(false);
    }

    public void TryToOpenMenu()
    {
        if (m_isInMenu)
        {
            CloseMenu();
            if (m_selectedButton != null)
                m_selectedButton.UnHighlightButton();
        }
        else
        {
            OpenMenu();
            if(m_selectedButton != null)
                m_selectedButton.HighlightButton();
        }
    }


    protected void OpenMenu()
    {
        m_isInMenu = true;

        menu.SetActive(true);
    }
    protected void CloseMenu()
    {
        m_isInMenu = false;

        menu.SetActive(false);
    }

    public void PressSelectedButton()
    {
        m_selectedButton.PressButton();
    }

    public void ChangeSelectedButton(Vector2 dir)
    {
        GamepadButton button = null;

        if (m_selectedButton != null)
        {
            button = m_selectedButton.ChangeButton(dir);
        }

        if(button != null)
        {
            m_selectedButton.UnHighlightButton();
            m_selectedButton=button;
            m_selectedButton.HighlightButton();

        }
    }
}
