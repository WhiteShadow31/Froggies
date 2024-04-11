using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(BoxCollider))]
public class GatePlant : MonoBehaviour
{
    public List<Transform> flowers;
    List<Quaternion> startRotation = new List<Quaternion>();

    private void Awake()
    {
        foreach(Transform t in flowers)
        {
            startRotation.Add(t.rotation);
        }
    }

    public void Open(float value)
    {
        for(int i = 0; i < flowers.Count; i++)
        {
            flowers[i].rotation = Quaternion.Lerp(startRotation[i], Quaternion.identity, value);
        }
    }

    public void TotalOpen()
    {
        Open(1);
        GetComponent<BoxCollider>().enabled = false;
    }

    public void Close()
    {
        for (int i = 0; i < flowers.Count; i++)
        {
            flowers[i].rotation = Quaternion.Lerp(startRotation[i], Quaternion.identity, 0);
        }
    }

}
