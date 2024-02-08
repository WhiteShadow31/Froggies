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


    protected GameObject canvasGO;

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
        if(PlayerClose())
            canvasGO.SetActive(true);
        else 
            canvasGO.SetActive(false);
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
        GameObject goChild = new GameObject("DialogueImage");
        goChild.transform.parent = canvasGO.transform;

        Image img = goChild.AddComponent<Image>();

        RectTransform rectImg = goChild.GetComponent<RectTransform>();
        rectImg.localPosition = offsetImage;
        rectImg.sizeDelta = sizeImage;

        if (spriteToShow == null)
            Debug.LogWarning("There is no sprite to show.");
        else
            img.sprite = spriteToShow;
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
        Gizmos.DrawCube(this.transform.position + offsetImage, new Vector3(sizeImage.x, sizeImage.y, 0.1f));
    }
}
