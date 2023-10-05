using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class InputTest : MonoBehaviour
{
    void OnMove(InputValue ctx)
    {
        Debug.Log(ctx.Get<Vector2>());
    }

    void OnStartAim()
    {
        Debug.Log("Start");
    }

    void OnEndAim()
    {
        Debug.Log("End");
    }
}
