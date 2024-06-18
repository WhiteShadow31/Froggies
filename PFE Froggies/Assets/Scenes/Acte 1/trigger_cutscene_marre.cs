using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class trigger_cutscene_marre : MonoBehaviour
{

    public GameObject cinematique;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }


    // Détecte les grenouilles
    void OnTriggerEnter(Collider hit_box) {

            cinematique.SetActive(true);
    }
}
