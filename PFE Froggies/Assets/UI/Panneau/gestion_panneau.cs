using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class gestion_panneau : MonoBehaviour
{

    // TMP Text
    public TMP_Text texte_save;
    public TMP_Text texte_niveau;
    
    // Autres
    public enum_save numero_save;
    public stock_enum_ui.nom_level Nom_Level;
    stock_enum_ui Stock_Enum_Ui = new stock_enum_ui();

    public enum enum_save {
        save_1, save_2, save_3, continuer
    };


    // Start is called before the first frame update
    void Start()
    {
        switch (numero_save) {

            case enum_save.save_1:
                texte_save.text = "Save 1";
            break;

            case enum_save.save_2:
                texte_save.text = "Save 2";
            break;

            case enum_save.save_3:
                texte_save.text = "Save 3";
            break;

            case enum_save.continuer:
                texte_save.text = "Continuer:";
            break;           
        }

        switch (Nom_Level) {
            
            case stock_enum_ui.nom_level.ecran_titre:
                texte_niveau.text = "Ecran Titre";
            break;

            case stock_enum_ui.nom_level.selection_niveau:
                texte_niveau.text = "Selection Niveaux";
            break;

            case stock_enum_ui.nom_level.lvl_1:
                texte_niveau.text = "Niveau 1";
            break;

            case stock_enum_ui.nom_level.lvl_2:
                texte_niveau.text = "Niveau 2";
            break;
        }

    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
