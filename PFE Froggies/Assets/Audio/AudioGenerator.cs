using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioGenerator : MonoBehaviour
{
    static AudioGenerator _instance;
    public static AudioGenerator Instance => _instance;

    private void Awake()
    {
        _instance = this;
    }
}
