using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SC_Tp : MonoBehaviour {

    float Nbplayer = 0;

    public void OnTriggerEnter(Collider other) 
    {
        if(other.transform.TryGetComponent<PlayerEntity>(out PlayerEntity player))
        {
            Nbplayer += 1;
        }

        
        if (Nbplayer >= 2 && SceneManager.GetActiveScene().buildIndex >= SceneManager.sceneCount-1) {

            SceneManager.LoadScene(0);

        } 
        else 
        {

            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex +1);
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