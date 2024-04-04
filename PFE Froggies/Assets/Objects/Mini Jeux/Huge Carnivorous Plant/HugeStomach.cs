using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HugeStomach : MonoBehaviour
{
    public HugeCarnivorousPlant hugeCarnivorousPlant;


    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.tag == "Food")
        {
            if(hugeCarnivorousPlant != null)
            {
                hugeCarnivorousPlant.actualFood += 1;
                hugeCarnivorousPlant.ActivateMouthRotation();
            }
            Destroy(other.gameObject);
        }
        else if(other.TryGetComponent<PlayerEntity>(out PlayerEntity player))
        {
            if ((hugeCarnivorousPlant.respawnPoint != null))
            {
                player.transform.position = hugeCarnivorousPlant.respawnPoint.position;
            }
        }
    }
}
