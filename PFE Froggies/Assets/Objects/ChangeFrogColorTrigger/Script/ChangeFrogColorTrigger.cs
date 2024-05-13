using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UltimateAttributesPack;

public class ChangeFrogColorTrigger : MonoBehaviour
{

    [SerializeField] LayerMask _playerLayerMask;
    [Space]
    [SerializeField] PrimaryColor _color;
    [SerializeField, Range(0,1)] float _colorPercentAdd;
    [SerializeField] float _stepTime;

    Color _targetColor;
    Color _colorAdd;
    bool _changingColor = false;
    float _stepTimer = 0f;
    List<PlayerColorChange> _playeColorChanges = new List<PlayerColorChange>();

    private void Start()
    {
        switch (_color)
        {
            case PrimaryColor.Red:
                _targetColor = Color.red;
                _colorAdd = new Color(_colorPercentAdd, 0, 0, 1);
                break;
            case PrimaryColor.Blue:
                _targetColor = Color.blue;
                _colorAdd = new Color(0, 0, _colorPercentAdd, 1);
                break;
            case PrimaryColor.Yellow:
                _targetColor = Color.yellow;
                _colorAdd = new Color(_colorPercentAdd, _colorPercentAdd, 0, 1);
                break;
        }
    }

    IEnumerator ChangeFrogColor(PlayerEntity player)
    {
        Renderer[] renderers = player.model.GetComponentsInChildren<Renderer>();
        float colorValue = 0f;
        switch (_color)
        {
            case PrimaryColor.Red:
                colorValue = renderers[0].sharedMaterial.color.r;
                break;
            case PrimaryColor.Blue:
                colorValue = renderers[0].sharedMaterial.color.b;
                break;
            case PrimaryColor.Yellow:
                colorValue = renderers[0].sharedMaterial.color.r > renderers[0].sharedMaterial.color.g ? renderers[0].sharedMaterial.color.r : renderers[0].sharedMaterial.color.g;
                break;
        }

        while (renderers[0].sharedMaterial.color !=  _targetColor)
        {
            yield return new WaitForSeconds(_stepTime);
            Debug.Log("next");
            Color newColor = renderers[0].sharedMaterial.color;
            newColor += _colorAdd;

            foreach(Renderer renderer in renderers)
            {
                renderer.sharedMaterial.color = newColor;
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if((_playerLayerMask.value & (1 << other.transform.gameObject.layer)) > 0)
        {
            if(other.TryGetComponent<PlayerEntity>(out PlayerEntity frogEntity))
            {
                PlayerColorChange newPlayerColorChange = new PlayerColorChange();
                newPlayerColorChange.Player = frogEntity.gameObject;
                newPlayerColorChange.Coroutine = StartCoroutine(ChangeFrogColor(frogEntity));   
                _playeColorChanges.Add(newPlayerColorChange);
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if ((_playerLayerMask.value & (1 << other.transform.gameObject.layer)) > 0)
        {
            if (other.TryGetComponent<PlayerEntity>(out PlayerEntity frogEntity) && _playeColorChanges.Count > 0)
            {
                foreach(PlayerColorChange playerColorChange in _playeColorChanges)
                {
                    if(playerColorChange.Player == frogEntity)
                    {
                        StopCoroutine(playerColorChange.Coroutine);
                        _playeColorChanges.Remove(playerColorChange);
                    }
                }
            }               
        }
    }

    class PlayerColorChange
    {
        public GameObject Player;
        public Coroutine Coroutine;
    }

    enum PrimaryColor
    {
        Red,
        Blue,
        Yellow
    }
}
