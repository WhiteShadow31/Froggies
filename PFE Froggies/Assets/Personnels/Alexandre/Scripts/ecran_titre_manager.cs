using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ecran_titre_manager : MonoBehaviour
{
    
    // Permet de quitter le jeu
    public void quitter() {

        Debug.Log("jeu quitter avec succès");
        Application.Quit();
    }


    // Permet de lancer le jeu
    public void jouer() {

        SceneManager.LoadScene("ecran_titre");
        Debug.Log("scène loaded");
    }
}
