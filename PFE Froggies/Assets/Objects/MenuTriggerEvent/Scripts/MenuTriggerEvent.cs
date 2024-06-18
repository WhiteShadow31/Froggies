using UnityEngine;
using UnityEngine.Events;
using UltimateAttributesPack;

public class MenuTriggerEvent : MonoBehaviour
{
    [SerializeField] GameObject _transitionObject;

    [LineTitle("Event")]
    [SerializeField] int _activateIfPlayersCountInTrigger;
    [SerializeField] float _activationTime;
    [SerializeField] Vector3 _activationTargetScale;
    [SerializeField] AnimationCurve _activationCurve;
    [SerializeField] LayerMask _playersLayerMask;
    [Space]
    [SerializeField] GameObject _textObject;
    [SerializeField] GameObject[] _objectsToActivate;
    [SerializeField] GameObject[] _objectsToDesactivate;
    [SerializeField] UnityEvent _event;
    [Space]
    [SerializeField] Vector3 _baseScale;
    [SerializeField] Color _baseColor;

    int _playersInTriggerCount = 0;
    float _activationTimer;

    [LineTitle("Transition")]
    [SerializeField] bool _hasTransition;
    [SerializeField, ShowIf(nameof(_hasTransition), true)] Vector3 _transitionTargetScale;
    [Space]
    [SerializeField, ShowIf(nameof(_hasTransition), true)] float _transitionOutTime;
    [SerializeField, ShowIf(nameof(_hasTransition), true)] Color _trasitionOutColor;
    [SerializeField, ShowIf(nameof(_hasTransition), true)] AnimationCurve _transitionOutCurve;
    [Space]
    [SerializeField, ShowIf(nameof(_hasTransition), true)] float _transitionInTime;
    [SerializeField, ShowIf(nameof(_hasTransition), true)] Color _trasitionInColor;
    [SerializeField, ShowIf(nameof(_hasTransition), true)] AnimationCurve _transitionInCurve;

    bool _inTransition;
    bool _isTransitionIn;
    float _transitionTimer;

    public enum TriggerType {Save, Play, Back}
    public TriggerType type;

    [Title("Debug")]
    [SerializeField] bool _showDebug;

    MeshRenderer _meshRenderer;

    private void Awake()
    {
        _meshRenderer = _transitionObject.GetComponent<MeshRenderer>();
    }

    private void Update()
    {
        // Activation
        if (!_inTransition)
        {
            if(_playersInTriggerCount == 2)
            {
                if(_activationTimer < _activationTime)
                    _activationTimer = Mathf.Clamp(_activationTimer += Time.deltaTime, 0, _activationTime);
                else
                    _inTransition = true;
            }
            else
                _activationTimer = Mathf.Clamp(_activationTimer -= Time.deltaTime, 0, _activationTime);

            // Lerp scale
            _transitionObject.transform.localScale = Vector3.Lerp(_baseScale, _activationTargetScale, _activationCurve.Evaluate(_activationTimer / _activationTime));
        }
        // Transition Out
        else if (_inTransition && _hasTransition)
        {
            if (!_isTransitionIn)
            {
                // Transition out
                if(_transitionTimer < _transitionOutTime)
                {
                    // Lerp scale and color
                    _transitionObject.transform.localScale = Vector3.Lerp(_activationTargetScale, _transitionTargetScale, _transitionOutCurve.Evaluate(_transitionTimer / _transitionOutTime));
                    _meshRenderer.sharedMaterial.color = Color.Lerp(_baseColor, _trasitionOutColor, _transitionOutCurve.Evaluate(_transitionTimer / _transitionOutTime));

                    _transitionTimer += Time.deltaTime;
                }
                else
                {
                    // Set scale and color to target
                    _transitionObject.transform.localScale = _transitionTargetScale;
                    _meshRenderer.sharedMaterial.color = _trasitionOutColor;

                    // Play events
                    PlayEvents();

                    // Activate transition in
                    _isTransitionIn = true;
                    _transitionTimer = 0f;
                }               
            }
            // Transition In
            else
            {
                // Transition in
                if (_transitionTimer < _transitionOutTime)
                {
                    // Lerp scale and color
                    _transitionObject.transform.localScale = Vector3.Lerp(_transitionTargetScale, Vector3.zero, _transitionOutCurve.Evaluate(_transitionTimer / _transitionOutTime));
                    _meshRenderer.sharedMaterial.color = Color.Lerp(_trasitionOutColor, _trasitionInColor, _transitionOutCurve.Evaluate(_transitionTimer / _transitionOutTime));

                    _transitionTimer += Time.deltaTime;
                }
                else
                {
                    // Set scale and color to target
                    _transitionObject.transform.localScale = Vector3.zero;
                    _meshRenderer.sharedMaterial.color = _trasitionInColor;

                    // Desactivate object
                    DesactivateObject();
                }
            }
        }
        // If there is no transition, then play events directly
        else
        {
            PlayEvents(); // Play events
            DesactivateObject(); // Desactivate object
        }
    }

    void PlayEvents()
    {
        // Activate all objects to activate
        foreach (GameObject go in _objectsToActivate)
        {
            go.SetActive(true);
            if(go.TryGetComponent<MenuTriggerEvent>(out MenuTriggerEvent script))
            {
                script.ResetObject();
            }
        }

        // Desactivate all objects to desactivate
        foreach (GameObject go in _objectsToDesactivate)
        {
            go.SetActive(false);
        }

        _textObject.SetActive(false); // Disable text
        _event.Invoke(); // Play events
    }

    void DesactivateObject()
    {
        gameObject.SetActive(false);
    }

    public void ResetObject()
    {
        _transitionObject.transform.localScale = _baseScale;
        _meshRenderer.sharedMaterial.color = _baseColor;
        _textObject.SetActive(true);

        _playersInTriggerCount = 0;
        _activationTimer = 0;
        _transitionTimer = 0;
        _inTransition = false;
        _isTransitionIn = false;
    }

    private void OnDrawGizmos()
    {
        if (_showDebug)
        {
            // Draw activation circle
            Gizmos.color = Color.blue;
            Gizmos.DrawWireSphere(_transitionObject.transform.position, _activationTargetScale.x / 2);

            if (_hasTransition)
            {
                // Draw transition circle
                Gizmos.color = Color.yellow;
                Gizmos.DrawWireSphere(_transitionObject.transform.position, _transitionTargetScale.x / 2);
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if ((_playersLayerMask.value & (1 << other.transform.gameObject.layer)) > 0)
        {
            _playersInTriggerCount++;

            _activationTimer = 0;
            _transitionTimer = 0f;

            if(_playersInTriggerCount == 2 && AudioGenerator.Instance != null)
            {
                switch(type)
                {
                    case TriggerType.Save:
                        AudioGenerator.Instance.PlaySaveAt(this.transform.position);
                        break;
                    case TriggerType.Play:
                        AudioGenerator.Instance.PlayClipAt(this.transform.position, "UI_Start", false, 1, 1, Camera.main.gameObject);
                        break;
                    case TriggerType.Back:
                        AudioGenerator.Instance.PlayClipAt(this.transform.position, "UI_Back", false, 1, 1, Camera.main.gameObject);
                        break;
                }
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if ((_playersLayerMask.value & (1 << other.transform.gameObject.layer)) > 0)
        {
            _playersInTriggerCount--;

            if(_playersInTriggerCount < 2 && type == TriggerType.Save)
            {
                if(AudioGenerator.Instance != null)
                {
                    AudioGenerator.Instance.StopSave();
                }
            }
        }
    }
}
