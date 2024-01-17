using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIRotateToCamera : MonoBehaviour
{
    protected Camera m_cam;

    protected virtual void Awake()
    {
        m_cam = Camera.main;
    }

    protected virtual void Update()
    {
        //this.transform.rotation = Quaternion.Slerp(this.transform.rotation, m_cam.transform.rotation, 1000f * Time.deltaTime);
        RotateToCam(this.transform, m_cam);
    }

    protected void RotateToCam(Transform tform, Camera cam)
    {
        tform.rotation = Quaternion.Slerp(tform.rotation, cam.transform.rotation, 1000f * Time.deltaTime);
    }
}
