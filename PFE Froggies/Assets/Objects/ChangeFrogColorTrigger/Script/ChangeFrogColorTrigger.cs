using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using UltimateAttributesPack;
using Unity.VisualScripting;

public class ChangeFrogColorTrigger : MonoBehaviour
{

    [SerializeField] LayerMask _playerLayerMask;
    [Space]
    [SerializeField] PrimaryColor _targetPrimaryColor;
    [SerializeField] float _maxChangeColorTime;

    Color _targetColor;
    [SerializeField] List<PlayerColorChange> _playerColorChanges = new List<PlayerColorChange>();

    private void Start()
    {
        switch(_targetPrimaryColor)
        {
            case PrimaryColor.Red:
                _targetColor = Color.red;
                break;
            case PrimaryColor.Yellow:
                _targetColor = new Color(1,1,0,1);
                break;
            case PrimaryColor.Blue:
                _targetColor = Color.blue;
                break;
        }
    }

    IEnumerator ChangeFrogColor(PlayerColorChange playerColorChange, Color startColor)
    {
        Renderer[] renderersToChange = playerColorChange.PlayerEntity.model.GetComponentsInChildren<Renderer>();
        
        Vector3 startColorValues = new Vector3(startColor.r, startColor.g, startColor.b);
        Vector3 targetColorValues = new Vector3(_targetColor.r, _targetColor.g, _targetColor.b);

        while (playerColorChange.ChangeColorTimer < playerColorChange.ChangeColorTime)
        {

            foreach(Renderer renderer in renderersToChange)
            {
                Vector3 newColorValues = LerpHSV(RGB2HSV(startColorValues), RGB2HSV(targetColorValues), playerColorChange.ChangeColorTimer / playerColorChange.ChangeColorTime);
                Vector3 newColor = HSV2RGB(newColorValues);
                renderer.sharedMaterial.color = new Color(newColor.x, newColor.y, newColor.z);
            }
            yield return new WaitForNextFrameUnit();
            playerColorChange.ChangeColorTimer += Time.deltaTime;
        }
        yield return null;
    }

    Vector3 RGB2HSV(Vector3 color)
    {
        float[] colorValues = { color.x, color.y, color.z };
        float cmax = Mathf.Max(colorValues);
        float cmin = Mathf.Min(colorValues);
        float delta = cmax - cmin;
        
        float hue = 0;
        float saturation = 0;

        if (cmax == color.x)
        {
            hue = 60 * (((color.y - color.z) / delta) % 6);
        }
        else if (cmax == color.y)
        {
            hue = 60 * ((color.z - color.x) / delta + 2);
        }
        else if (cmax == color.y)
        {
            hue = 60 * ((color.x - color.y) / delta + 4);
        }
        if (cmax > 0)
        {
            saturation = delta / cmax;
        }

        return new Vector3(hue, saturation, cmax);
    }

    Vector3 HSV2RGB(Vector3 color)
    {
        float hue = color.x;
        float c = color.z * color.y;
        float x = c * (1 - Mathf.Abs((hue / 60) % 2 - 1));
        float m = color.z - c;

        float r = 0;
        float g = 0;
        float b = 0;

        if (hue < 60)
        {
            r = c;
            g = x;
        }
        else if (hue < 120)
        {
            r = x;
            g = c;
        }
        else if (hue < 180)
        {
            g = c;
            b = x;
        }
        else if (hue < 240)
        {
            g = x;
            b = c;
        }
        else if (hue < 300)
        {
            r = x;
            b = c;
        }
        else
        {
            r = c;
            b = x;
        }

        return new Vector3(r + m, g + m, b + m);
    }

    Vector3 LerpHSV(Vector3 a, Vector3 b, float x)
    {
        Vector3 ah = RGB2HSV(a);
        Vector3 bh = RGB2HSV(b);
        Vector3 color = new Vector3(
            Mathf.LerpAngle(ah.x, bh.x, x),
            Mathf.Lerp(ah.y, bh.y, x),
            Mathf.Lerp(ah.z, bh.z, x)
        );
        return new Vector3(color.x, color.y, color.z);
    }

    float CalculateColorChangeTime(Color startColor)
    {
        float percent = 0f;

        switch (_targetPrimaryColor)
        {
            case PrimaryColor.Red:
                percent = Mathf.InverseLerp(0, 1, startColor.r);
                break;
            case PrimaryColor.Yellow:
                percent = Mathf.InverseLerp(0, 1, Mathf.Min(startColor.r, startColor.g));               
                break;
            case PrimaryColor.Blue:
                percent = Mathf.InverseLerp(0, 1, startColor.b);
                break;
        }

        return Mathf.Lerp(0, _maxChangeColorTime, percent);
    }

    private void OnTriggerEnter(Collider other)
    {
        if((_playerLayerMask.value & (1 << other.transform.gameObject.layer)) > 0)
        {
            if(other.TryGetComponent<PlayerEntity>(out PlayerEntity frogEntity))
            {
                Renderer[] frogRenderers = frogEntity.model.GetComponentsInChildren<Renderer>();               

                PlayerColorChange newPlayerColorChange = new PlayerColorChange();
                newPlayerColorChange.PlayerEntity = frogEntity;
                newPlayerColorChange.ChangeColorTime = 2f; //CalculateColorChangeTime(frogRenderers[0].sharedMaterial.color);
                newPlayerColorChange.ChangeColorTimer = 0f;
                newPlayerColorChange.Coroutine = StartCoroutine(ChangeFrogColor(newPlayerColorChange, frogRenderers[0].sharedMaterial.color));

                _playerColorChanges.Add(newPlayerColorChange);
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if ((_playerLayerMask.value & (1 << other.transform.gameObject.layer)) > 0)
        {
            if (other.TryGetComponent<PlayerEntity>(out PlayerEntity frogEntity) && _playerColorChanges.Count > 0)
            {
                foreach(PlayerColorChange playerColorChange in _playerColorChanges)
                {
                    if(playerColorChange.PlayerEntity == frogEntity)
                    {
                        StopCoroutine(playerColorChange.Coroutine);
                        playerColorChange.ChangeColorTimer = 0f;
                        _playerColorChanges.Remove(playerColorChange);
                        break;
                    }
                }
            }               
        }
    }

    enum PrimaryColor
    {
        Red,
        Yellow,
        Blue
    }

    [Serializable]
    class PlayerColorChange
    {
        public PlayerEntity PlayerEntity;
        public Coroutine Coroutine;
        public float ChangeColorTime;
        public float ChangeColorTimer;
    }
}
