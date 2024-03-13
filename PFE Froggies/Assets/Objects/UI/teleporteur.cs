using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class teleporteur : MonoBehaviour
{

    // Enumération des différents niveaux possibles
    public enum nom_level {
        ecran_titre, selection_niveau, lvl_1, lvl_2
    };

    public Collider hit_box;
    int grenouilles_check;
    public nom_level Nom_Level;


    // Permet de faire des trucs au lancement du script
    void Start() {
        grenouilles_check = 0;
    }


    // Permet de détexter quand une frog entre dans le collider
    void OnTriggerEnter(Collider hit_box) {
        
        grenouilles_check += 1;
        Debug.Log(grenouilles_check);

        verif_assez_grenouilles();
    }


    // Permet de détexter quand une frog sort du collider
    void OnTriggerExit(Collider hit_box) {
        
        grenouilles_check -= 1;
        Debug.Log(grenouilles_check);
    }


    // Permet de vérifier si les deux grenouilles sont dans le téléporteur
    void verif_assez_grenouilles() {

        if (grenouilles_check == 2) {
            changement_scene();
        }
    }


    // Permet d'ouvrir la scène selon la valeur choisi dans l'éditeur
    void changement_scene() {

        switch (Nom_Level) {

            case nom_level.ecran_titre:
                SceneManager.LoadScene("ecran_titre");
            break;

            case nom_level.selection_niveau:
                SceneManager.LoadScene("selection_niveaux");
            break;

            case nom_level.lvl_1:
                SceneManager.LoadScene("Village des brunes");
            break;

            case nom_level.lvl_2:
                SceneManager.LoadScene("Foret cool");
            break;
        }
    }
}
