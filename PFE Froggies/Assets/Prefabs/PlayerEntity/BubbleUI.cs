using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BubbleUI : MonoBehaviour
{
    [SerializeField]
    protected CameraEntity _cameraEntity;

    private void Awake()
    {
        if (_cameraEntity == null)
        {
            _cameraEntity = Camera.main.GetComponent<CameraEntity>();
        }
    }

    private void Start()
    {
        if (_cameraEntity == null)
        {
            _cameraEntity = Camera.main.GetComponent<CameraEntity>();
        }
    }

    private void Update()
    {
        this.transform.LookAt(_cameraEntity.transform);
    }
}
