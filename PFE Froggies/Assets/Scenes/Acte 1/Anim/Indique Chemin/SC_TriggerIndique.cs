using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SC_TriggerIndique : MonoBehaviour {

    public Animator Indique;

    // Start is called before the first frame update
    void Start() {


        Indique = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update() {
        
    }

    public void OnTriggerEnter(Collider other) {

        if (Indique != null) {

            Indique.Play("Indique", 0, 0.25f);
            Indique.Play("Indique 1", 0, 0.25f);
        }
    }
}