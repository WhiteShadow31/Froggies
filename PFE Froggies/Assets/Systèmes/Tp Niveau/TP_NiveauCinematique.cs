using System.Collections;
using System.Collections.Generic;
using UltimateAttributesPack;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TP_NiveauCinematique : MonoBehaviour
{
    [SerializeField, Scene] string _targetScene;
    public EndSceneTransition endTransition;

    // Start is called before the first frame update
    void Start()
    {
        if (endTransition != null)
        {
            endTransition.Fade(_targetScene);
        }
        else
            SceneManager.LoadScene(_targetScene);
    }

    // Update is called once per frame
    void Update()
        {
        }
}