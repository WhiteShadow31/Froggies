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
    [SerializeField] float _targetHue;
    [SerializeField] float _maxChangeColorTime;
    [SerializeField] List<PlayerColorChange> _playerColorChanges = new List<PlayerColorChange>();

    private void Start()
    {
        // Set target hue from 0 and 1
        _targetHue = _targetHue / 359f;
    }

    IEnumerator ChangeFrogColor(PlayerColorChange playerColorChange, float startHue)
    {
        Renderer[] renderersToChange = playerColorChange.PlayerEntity.model.GetComponentsInChildren<Renderer>();

        while (playerColorChange.ChangeColorTimer < playerColorChange.ChangeColorTime)
        {
            float currentHue = LerpHue(startHue, _targetHue, playerColorChange.ChangeColorTimer / playerColorChange.ChangeColorTime);

            Color targetColor = Color.HSVToRGB(currentHue, 1f, 1f);
            Material newMat = new Material(renderersToChange[0].sharedMaterial);
            newMat.color = targetColor;

            foreach (Renderer renderer in renderersToChange)
            {
                renderer.material = newMat;
                playerColorChange.PlayerEntity.playerColor = targetColor;
            }
            yield return new WaitForNextFrameUnit();
            playerColorChange.ChangeColorTimer += Time.deltaTime;
        }
        yield return null;
    }

    float LerpHue(float start, float end, float t)
    {
        float shortestRed = Mathf.Abs(0f - start) < Mathf.Abs(1f - start) ? 0f : 1f;
        float longestRed = shortestRed == 0f ? 1f : 0f;
        if (Mathf.Abs(shortestRed - start) + Mathf.Abs(end - longestRed) < Mathf.Abs(end - start))
        {
            float d = Mathf.Abs(shortestRed - start) + Mathf.Abs(end - longestRed);
            float passedShortestRedPercent = Mathf.InverseLerp(0f, d, Mathf.Abs(shortestRed - start));

            if(t <= passedShortestRedPercent)
            {
                end = shortestRed;
                t = t / passedShortestRedPercent;
            }
            else
            {
                start = longestRed;
            }           
        }

        return Mathf.Lerp(start, end, t);
    }

    private void OnTriggerEnter(Collider other)
    {
        if((_playerLayerMask.value & (1 << other.transform.gameObject.layer)) > 0)
        {
            if(other.TryGetComponent<PlayerEntity>(out PlayerEntity frogEntity))
            {
                // Get frog HSV color
                Renderer[] frogRenderers = frogEntity.model.GetComponentsInChildren<Renderer>();
                Color.RGBToHSV(frogRenderers[0].sharedMaterial.color, out float frogH, out float frogS, out float frogV);

                // Set new player color change class
                PlayerColorChange newPlayerColorChange = new PlayerColorChange();
                newPlayerColorChange.PlayerEntity = frogEntity;
                newPlayerColorChange.ChangeColorTime = 2f;
                newPlayerColorChange.ChangeColorTimer = 0f;
                newPlayerColorChange.Coroutine = StartCoroutine(ChangeFrogColor(newPlayerColorChange, frogH));

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

    [Serializable]
    class PlayerColorChange
    {
        public PlayerEntity PlayerEntity;
        public Coroutine Coroutine;
        public float ChangeColorTime;
        public float ChangeColorTimer;
    }
}
