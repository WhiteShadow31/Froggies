using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UltimateAttributesPack;

public class SC_Tp : MonoBehaviour {

    [SerializeField, Scene] string _targetScene;

    float Nbplayer = 0;

    public void OnTriggerEnter(Collider other) 
    {
        if(other.transform.TryGetComponent<PlayerEntity>(out PlayerEntity player))
        {
            Nbplayer += 1;
        }
       
        if (Nbplayer >= 2) {
            SceneManager.LoadScene(_targetScene);
        } 
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.transform.TryGetComponent<PlayerEntity>(out PlayerEntity player))
        {
            Nbplayer -= 1;
        }
    }
}