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
        if(_button == null)
            _button = GetComponent<Button>();
        _button.Select();
    }

    public void UnHighlightButton()
    {
        if (EventSystem.current.currentSelectedGameObject == _button)
        {
        }
            EventSystem.current.SetSelectedGameObject(null);
    }

    public GamepadButton ChangeButton(Vector2 dir)
    {
        // UP DOWN
        if (Mathf.Abs(dir.x) < Mathf.Abs(dir.y))
        {
            if (dir.y < 0)
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

    public virtual void PressButton()
    {
        Debug.Log(this.gameObject.name);
        _button.onClick.Invoke();
    }
}
