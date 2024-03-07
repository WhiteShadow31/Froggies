using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Menu : MonoBehaviour
{
    public GameObject menu;
    [Space]
    public GamepadButton m_starterButton;
    [SerializeField] public GamepadButton m_selectedButton;

    public void OpenMenu()
    {
        MenuManager.Instance.actualMenu.CloseMenu();
        MenuManager.Instance.actualMenu = this;

        if(menu != null)
        {
            menu.SetActive(true);
        }

        if(m_starterButton != null)
        {
            if(m_selectedButton != null)
            {
                m_selectedButton.UnHighlightButton();
            }
            m_starterButton.HighlightButton();

            m_selectedButton = m_starterButton;
        }

        
    }

    public void CloseMenu()
    {
        if (menu != null)
        {
            if (m_starterButton != null)
            {
                if (m_selectedButton != null)
                {
                    m_selectedButton.UnHighlightButton();
                }
                m_starterButton.HighlightButton();

                //m_selectedButton = m_starterButton;
            }

            menu.SetActive(false);
        }
    }

    public void PressSelectedButton()
    {
        if (m_selectedButton != null)
        {
            Debug.Log("The selected button to be pressed is " + m_selectedButton.name);
            m_selectedButton.PressButton();
        }
    }

    public void ChangeSelectedButton(Vector2 dir)
    {
        GamepadButton button = null;

        if (m_selectedButton != null)
        {
            button = m_selectedButton.ChangeButton(dir);
        }

        if (button != null)
        {
            m_selectedButton.UnHighlightButton();
            button.HighlightButton();
            m_selectedButton = button;

        }
    }
}
