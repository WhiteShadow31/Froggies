using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class teleporteur : MonoBehaviour
{

    // Variables
    public Collider hit_box;
    int grenouilles_check;
    stock_enum_ui Stock_Enum_Ui = new stock_enum_ui();
    
    public stock_enum_ui.nom_level Nom_Level;


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

            case stock_enum_ui.nom_level.ecran_titre:
                SceneManager.LoadScene("ecran_titre");
            break;

            case stock_enum_ui.nom_level.selection_niveau:
                SceneManager.LoadScene("selection_niveaux");
            break;

            case stock_enum_ui.nom_level.lvl_1:
                SceneManager.LoadScene("Village des brunes");
            break;

            case stock_enum_ui.nom_level.lvl_2:
                SceneManager.LoadScene("Foret cool");
            break;
        }
    }
}
