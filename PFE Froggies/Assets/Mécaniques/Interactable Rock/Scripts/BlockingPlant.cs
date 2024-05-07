using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlockingPlant : InteractableDuoEntity, IInteractableEntity
{
    private void Awake()
    {
        GetComponent<Rigidbody>().isKinematic = true;
    }

    public override void Push(Vector3 dir, float force, GameObject frog)
    {
        GameObject model = this.transform.Find("Model").gameObject;
        model.SetActive(false);
        this.transform.GetComponent<Collider>().enabled = false;
    }
}
