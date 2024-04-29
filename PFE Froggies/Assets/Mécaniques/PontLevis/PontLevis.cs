using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PontLevis : MonoBehaviour
{
    public Transform transformToRotate;
    public float angleToRotate = 90f;
    public float timeToRotate = 1;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    IEnumerator RotatePont()
    {
        float time = 0;
        Vector3 eulers = transformToRotate.localEulerAngles;
        float startXEuler = eulers.x;

        while (time < timeToRotate)
        {
            float xEuler = Mathf.Lerp(startXEuler, angleToRotate, time / timeToRotate);
            transformToRotate.localRotation = Quaternion.Euler(new Vector3(xEuler, eulers.y, eulers.z));
            time += Time.fixedDeltaTime;

            yield return null;
        }
        transformToRotate.localRotation = Quaternion.Euler(new Vector3(angleToRotate, eulers.y, eulers.z));
    }

    public void ActivatePont()
    {
        StartCoroutine(RotatePont());
    }
}
