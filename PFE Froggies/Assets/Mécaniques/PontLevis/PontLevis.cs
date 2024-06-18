using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PontLevis : MonoBehaviour
{
    public Transform transformToRotate;
    public float angleToRotate = 90f;
    public float timeToRotate = 1;
    public Transform particlesPosition;

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

        if (ParticlesGenerator.Instance != null)
            ParticlesGenerator.Instance.PlayPontLevisHitParticles(this.transform.position);
        if (AudioGenerator.Instance != null)
            AudioGenerator.Instance.PlayClipAt(this.transform.position, "ENGM_Tronc_0"+Random.Range(1, 4));
    }

    public void ActivatePont()
    {
        StartCoroutine(RotatePont());
    }
}
