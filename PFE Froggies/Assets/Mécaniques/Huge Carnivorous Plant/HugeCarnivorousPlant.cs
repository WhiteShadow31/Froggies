using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HugeCarnivorousPlant : MonoBehaviour
{
    public HugeStomach stomach;
    public float timeToCloseMouth = 0.6f;
    public int nbrFoodMAX = 3;
    public int actualFood = 0;
    public Transform transformToRotate;
    public Transform respawnPoint;

    private void Start()
    {
        stomach.hugeCarnivorousPlant = this;
    }

    IEnumerator RotateMouth()
    {
        float time = 0;
        Vector3 eulers = transformToRotate.localEulerAngles;
        float startXEuler = eulers.x;

        while (time < timeToCloseMouth)
        {
            float xEuler = Mathf.Lerp(startXEuler, (90/nbrFoodMAX) * actualFood, time / timeToCloseMouth);
            transformToRotate.localRotation = Quaternion.Euler(new Vector3(xEuler, eulers.y, eulers.z));
            time += Time.fixedDeltaTime;

            yield return null;
        }
        transformToRotate.localRotation = Quaternion.Euler(new Vector3((90 / nbrFoodMAX) * actualFood, eulers.y, eulers.z));
    }

    public void ActivateMouthRotation()
    {
        StartCoroutine(RotateMouth());
    }
}
