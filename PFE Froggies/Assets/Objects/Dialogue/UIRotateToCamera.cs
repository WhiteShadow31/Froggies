using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIRotateToCamera : MonoBehaviour
{
    protected Camera m_cam;

    private void Awake()
    {
        m_cam = Camera.main;
    }

    private void Update()
    {
        this.transform.rotation = Quaternion.Slerp(this.transform.rotation, m_cam.transform.rotation, 1000f * Time.deltaTime);
    }
}
