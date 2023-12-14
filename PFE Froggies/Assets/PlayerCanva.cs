using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCanva : MonoBehaviour
{
    public static PlayerCanva Instance;

    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        Instance = this;
    }
}
