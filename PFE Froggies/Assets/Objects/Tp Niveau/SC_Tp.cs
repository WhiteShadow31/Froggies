using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SC_Tp : MonoBehaviour {

    float Nbplayer = 0;

    public void OnTriggerEnter(Collider other) {

        Nbplayer += 1;

        if (Nbplayer >= 2 && SceneManager.GetActiveScene().name == "N_Tuto") {

            SceneManager.LoadScene(1);

        } else {

            SceneManager.LoadScene(0);
        }
    }
}