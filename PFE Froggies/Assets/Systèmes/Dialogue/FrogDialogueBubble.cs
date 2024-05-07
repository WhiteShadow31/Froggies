using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FrogDialogueBubble : MonoBehaviour
{
    protected float m_maxDezoom = 25f;
    [SerializeField] protected Canvas m_canvas;
    [SerializeField] protected RectTransform m_bubbleRect;

    [SerializeField] protected bool m_showBubble = false;

    private void Awake()
    {
        if(m_canvas == null && this.transform.Find("BubbleCanvas").TryGetComponent<Canvas>(out Canvas bubbleCanvas)) // -- If no canvas referenced search for one
        {

            m_canvas = bubbleCanvas;
        }

        if (m_canvas != null && m_canvas.transform.Find("Bubble").TryGetComponent<RectTransform>(out RectTransform rect)) // -- If no canvas referenced search for one
            m_bubbleRect = rect;

        ShowBubble(true); // Hide the canvas
    }

    private void Update()
    {
        if (m_showBubble)
        {
            Vector2 bubblePos = GetPositionOnScreen();
            m_bubbleRect.position = bubblePos;
        }

    }


    public void ShowBubble(bool value)
    {
        if (m_canvas != null) // -- There is a canvas
        {
            m_canvas.gameObject.SetActive(value);
            if(value && m_bubbleRect)
            {
                Vector2 bubblePos = GetPositionOnScreen();
                m_bubbleRect.localPosition = bubblePos;
            }
        }

        m_showBubble =value;
    }

    protected Vector2 GetPositionOnScreen()
    {
        Camera cam = Camera.main;
        Vector2 position = cam.WorldToScreenPoint(this.transform.position);
        float ratio = ScaleTheBubble();
        position.y += 100 * ratio;

        return position;
    }

    protected float ScaleTheBubble()
    {
        float ratio = 1;

        Vector3 camPos = Camera.main.transform.position;
        float camDistanceToThis = camPos.y - this.transform.position.y;


        if(camDistanceToThis > m_maxDezoom*2)
        {
            ratio = 0;
        }
        else if(camDistanceToThis > m_maxDezoom)
        {
            ratio = 2-(camDistanceToThis / m_maxDezoom);
        }

        return ratio;
    }
}
