using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuManager : MonoBehaviour
{
    public static MenuManager Instance;
    public GameObject menu;

    // Button actually selected by the players
    protected GamepadButton m_selectedButton;
    public GamepadButton SelectedButton { set { m_selectedButton = value; } }

    private void Awake()
    {
        Instance = this;
    }

    public void PressSelectedButton()
    {
        m_selectedButton.PressButton();
    }

    public void ChangeSelectedButton(Vector2 dir)
    {
        m_selectedButton.UnHighlightButton();
        m_selectedButton = m_selectedButton.ChangeButton(dir);
        m_selectedButton.HighlightButton();
    }
}
