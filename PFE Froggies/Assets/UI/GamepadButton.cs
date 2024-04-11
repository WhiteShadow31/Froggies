using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

[RequireComponent(typeof(Button))]
public class GamepadButton : MonoBehaviour
{
    [Header("Button to link")]
    public GamepadButton upButton;
    public GamepadButton rightButton;
    public GamepadButton leftButton;
    public GamepadButton downButton;

    Button _button;

    private void Awake()
    {
        _button = GetComponent<Button>();
    }

    public void HighlightButton()
    {
        Debug.Log("Highlight");
        _button.Select();
    }

    public void UnHighlightButton()
    {
        if (EventSystem.current.currentSelectedGameObject == _button)
        {
            EventSystem.current.SetSelectedGameObject(null);
        }
    }

    public GamepadButton ChangeButton(Vector2 dir)
    {
        
        if(Mathf.Abs(dir.x) > 0.7f || Mathf.Abs(dir.y) > 0.7f)
        {
            // UP DOWN
            if(Mathf.Abs(dir.x) < Mathf.Abs(dir.y))
            {
                if(dir.y < 0)
                {
                    // Change down
                    return downButton;
                }
                else
                {
                    // Change up
                    return upButton;
                }
            }
            // LEFT RIGHT
            else
            {
                if (dir.x < 0)
                {
                    // Change left
                    return leftButton;
                }
                else
                {
                    // Change right
                    return rightButton;
                }
            }
        }
        //UnHighlightButton();

        return null;
    }

    public void PressButton()
    {
        _button.onClick.Invoke();
    }
}
