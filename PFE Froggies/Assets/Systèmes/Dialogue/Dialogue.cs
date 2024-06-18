using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Dialogue : UIRotateToCamera
{
    public Sprite spriteToShow;
    [Space]
    public float radiusDetection = 3f;
    [Header("Canvas Parameters")]
    public Vector3 offsetCanva = Vector3.zero;
    public Vector2 sizeCanva = new Vector2(1, 1);
    [Header("Image Parameters")]
    public Vector3 offsetImage = Vector3.zero;
    public Vector2 sizeImage = new Vector2(1,1);


    [Header("Animation Parameters")]
    [SerializeField] float _appearTime;
    [SerializeField] AnimationCurve _appearCurve;
    float _appearTimer = 0f;
    [SerializeField] float _disappearTime;
    [SerializeField] AnimationCurve _disappearCurve;
    float _disappearTimer = 0f;

    protected GameObject canvasGO;
    GameObject _bubbleGo;
    Image _bubbleImage;

    bool _useSound = false;

    protected override void Awake()
    {
        base.Awake();

        CreateUI();

        canvasGO.SetActive(false);
    }

    protected override void Update()
    {
        //base.Update();
        RotateToCam(canvasGO.transform, m_cam);
    }

    private void FixedUpdate()
    {
        if (PlayerClose())
        {
            _disappearTimer = 0f;

            _bubbleGo.transform.localScale = Vector2.zero;
            SetImageAlpha(1f);
            canvasGO.SetActive(true);

            if(_appearTimer < _appearTime)
            {
                _appearTimer += Time.deltaTime;
                _bubbleGo.transform.localScale = Vector2.Lerp(Vector2.zero, Vector2.one, _appearCurve.Evaluate(_appearTimer / _appearTime));
            }
            else
            {
                _bubbleGo.transform.localScale = Vector2.one;

                if (!_useSound)
                {
                    // POP SOUND
                    if(AudioGenerator.Instance != null)
                    {
                        AudioGenerator.Instance.PlayClipAt(this.transform.position, "UI_Dialogue_Pop");
                    }
                    _useSound = true;

                }
            }

        }
        else
        {
            _appearTimer = 0f;

            Color newColor = _bubbleImage.color;

            if (_disappearTimer < _disappearTime)
            {        
                _disappearTimer += Time.deltaTime;
                SetImageAlpha(Mathf.Lerp(1f, 0f, _disappearCurve.Evaluate(_disappearTimer / _disappearTime)));
            }
            else
            {
                SetImageAlpha(0f);
                canvasGO.SetActive(false);
                if (_useSound)
                {
                    // POP SOUND
                    if (AudioGenerator.Instance != null)
                    {
                        AudioGenerator.Instance.PlayClipAt(this.transform.position, "UI_Dialogue_Depop");
                    }
                    _useSound = false;

                }
            }

        }
    }

    void SetImageAlpha(float value)
    {
        Color color = _bubbleImage.color;
        color.a = value;
        _bubbleImage.color = color;
    }

    protected void CreateUI()
    {
        // CANVA
        canvasGO = new GameObject("Canvas");
        canvasGO.transform.parent = this.transform;

        Canvas canvas = canvasGO.AddComponent<Canvas>();
        canvas.renderMode = RenderMode.WorldSpace;

        RectTransform rectCanva = canvasGO.GetComponent<RectTransform>();
        rectCanva.localPosition = offsetCanva;
        rectCanva.sizeDelta = sizeCanva;
        
        // IMAGE
        _bubbleGo = new GameObject("DialogueImage");
        _bubbleGo.transform.parent = canvasGO.transform;

        _bubbleImage = _bubbleGo.AddComponent<Image>();

        RectTransform rectImg = _bubbleGo.GetComponent<RectTransform>();
        rectImg.localPosition = offsetImage;
        rectImg.sizeDelta = sizeImage;

        if (spriteToShow == null)
            Debug.LogWarning("There is no sprite to show.");
        else
            _bubbleImage.sprite = spriteToShow;
    }

    protected bool PlayerClose()
    {
        Collider[] cols = Physics.OverlapSphere(transform.position, radiusDetection);

        for(int i = 0; i < cols.Length; i++)
        {
            if (cols[i].gameObject.TryGetComponent<PlayerEntity>(out PlayerEntity player))
                return true;
        }

        return false;
    }

    private void OnDrawGizmosSelected()
    {
        Color color = Color.white;
        color.a = 0.3f;
        Gizmos.color = color;
        Gizmos.DrawCube(this.transform.position + offsetCanva, new Vector3(sizeCanva.x, sizeCanva.y, 0.1f));

        color = Color.red;
        color.a = 0.3f;
        Gizmos.color = color;
        Gizmos.DrawCube(this.transform.position + offsetImage, new Vector3(sizeImage.x, sizeImage.y, 0.1f));
    }
}
