using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class gestion_panneau : MonoBehaviour
{

    public TMP_Text texte_save;
    public TMP_Text texte_niveau;
    public enum_save numero_save;
    stock_enum_ui Stock_Enum_Ui = new stock_enum_ui();

    public enum enum_save {
        save_1, save_2, save_3
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
        }

        texte_niveau.text = "Niveau 1";
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
