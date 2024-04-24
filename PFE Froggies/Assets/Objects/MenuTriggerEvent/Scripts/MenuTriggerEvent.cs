using UnityEngine;
using UnityEngine.Events;
using UltimateAttributesPack;

public class MenuTriggerEvent : MonoBehaviour
{
    [LineTitle("Event")]
    [SerializeField] int _triggerIfPlayersInCount;
    [SerializeField] float _triggerAfter;
    [SerializeField] LayerMask _playersLayerMask;
    [SerializeField] GameObject[] _objectsToActivate;
    [SerializeField] GameObject[] _objectsToDesactivate;
    [SerializeField] UnityEvent _event;

    int _playersInTriggerCount = 0;
    float _triggerTimer;

    [LineTitle("Transition")]
    [SerializeField] GameObject _transitionObject;
    [SerializeField] Vector3 _transitionTargetScale;
    [Space]
    [SerializeField] float _transitionOutTime;
    [SerializeField] Color _trasitionOutColor;
    [SerializeField] AnimationCurve _transitionOutCurve;
    [Space]
    [SerializeField] float _transitionInTime;
    [SerializeField] Color _trasitionInColor;
    [SerializeField] AnimationCurve _transitionInCurve;

    Color _baseColor;
    Vector3 _baseScale;
    bool _inTransition;
    bool _isTransitionIn;
    float _transitionTimer;

    MeshRenderer _meshRenderer;

    private void Awake()
    {
        _meshRenderer = _transitionObject.GetComponent<MeshRenderer>();
    }

    private void Start()
    {
        _baseScale = _transitionObject.transform.localScale;
        _baseColor = _meshRenderer.sharedMaterial.color;
    }

    private void Update()
    {
        if(_playersInTriggerCount == _triggerIfPlayersInCount || _inTransition)
        {
            if (_triggerTimer < _triggerAfter)
            {
                _triggerTimer += Time.deltaTime;
            }
            else if(!_isTransitionIn)
            {
                _inTransition = true;
                
                // Transition out
                if(_transitionTimer < _transitionOutTime)
                {
                    _transitionObject.transform.localScale = Vector3.Lerp(_baseScale, _transitionTargetScale, _transitionOutCurve.Evaluate(_transitionTimer / _transitionOutTime));
                    _meshRenderer.sharedMaterial.color = Color.Lerp(_baseColor, _trasitionOutColor, _transitionOutCurve.Evaluate(_transitionTimer / _transitionOutTime));

                    _transitionTimer += Time.deltaTime;
                }
                else
                {
                    _transitionObject.transform.localScale = _transitionTargetScale;
                    _meshRenderer.sharedMaterial.color = _trasitionOutColor;

                    // Play events
                    ActivateTargetObjects();
                    DesactivateTargetObjects();
                    _event.Invoke();

                    _isTransitionIn = true;
                    _transitionTimer = 0f;
                }               
            }
            else
            {
                // Transition in
                if (_transitionTimer < _transitionOutTime)
                {
                    _transitionObject.transform.localScale = Vector3.Lerp(_transitionTargetScale, Vector3.zero, _transitionOutCurve.Evaluate(_transitionTimer / _transitionOutTime));
                    _meshRenderer.sharedMaterial.color = Color.Lerp(_trasitionOutColor, _trasitionInColor, _transitionOutCurve.Evaluate(_transitionTimer / _transitionOutTime));

                    _transitionTimer += Time.deltaTime;
                }
                else
                {
                    _transitionObject.transform.localScale = Vector3.zero;
                    _meshRenderer.sharedMaterial.color = _trasitionInColor;

                    // Desactivate object
                    DesactivateObject();
                }
            }
        }
    }

    void ActivateTargetObjects()
    {
        foreach(GameObject go in _objectsToActivate)
        {
            go.SetActive(true);
        }
    }

    void DesactivateTargetObjects()
    {
        foreach (GameObject go in _objectsToDesactivate)
        {
            go.SetActive(true);
        }
    }

    void DesactivateObject()
    {
        gameObject.SetActive(false);
    }

    private void OnTriggerEnter(Collider other)
    {
        if ((_playersLayerMask.value & (1 << other.transform.gameObject.layer)) > 0)
        {
            _playersInTriggerCount++;

            _triggerTimer = 0;
            _transitionTimer = 0f;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if ((_playersLayerMask.value & (1 << other.transform.gameObject.layer)) > 0)
        {
            _playersInTriggerCount--;
        }
    }
}
