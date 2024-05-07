using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Food : MonoBehaviour
{
    private void Awake()
    {
        this.gameObject.tag = "Food";
    }
}
