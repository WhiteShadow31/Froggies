using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SC_PlateformeCassante : MonoBehaviour {

    public float ChronoHide = 5f;
    public float ChronoShow = 3f;

    bool IsCoroutine = false;

    BoxCollider Box;
    MeshRenderer Mesh;

    private void Awake() {
        
        Box = GetComponent<BoxCollider>();
        Mesh = GetComponent<MeshRenderer>();
    }

    private void OnCollisionEnter(Collision collision) {

        if (!IsCoroutine) {

            StartCoroutine(Hide());
        }
    }

    private IEnumerator Hide() {

        IsCoroutine = true;
        yield return new WaitForSeconds(ChronoHide);
        SetPlateformState(false);
        StartCoroutine(Show());
        yield return null;
    }

    private IEnumerator Show() {

        IsCoroutine = false;
        yield return new WaitForSeconds(ChronoShow);
        SetPlateformState(true);
        yield return null;
    }

    void SetPlateformState(bool State) {

        Box.enabled = State;
        Mesh.enabled = State;
    }
}