using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButtonPhoto : MonoBehaviour, IInteractableEntity
{
    public Transform photo;
    public MeshRenderer opacity;
    public MeshRenderer photoTaken;
    [Header("Material")]
    public Material transparentMat;
    public Material renderMat;
    public float timeToShow = 1;
    bool _hasBeenUsed = false;
    public Camera photoCam;
    public RenderTexture rt;

    Texture2D tex2D;

    private void Awake()
    {
        photoCam.targetTexture = rt;

        photoTaken.material = renderMat;

        //renderMat.SetTexture("_MainTex", tex2D);

        //tex2D = toTexture2D(rt);
        renderMat.mainTexture = toTexture2D(rt);
        //Push(Vector3.zero, 0, null);
        //rt.Release();

    }
    public void Push(Vector3 dir, float force, GameObject frog)
    {
        if(!_hasBeenUsed)
        {
            if(AudioGenerator.Instance != null)
                AudioGenerator.Instance.PlayClipAt(this.transform.position, "ENGM_Appareil_O1");

            StartCoroutine(ShowPhoto());
            _hasBeenUsed = true;
        }
    }

    IEnumerator ShowPhoto()
    {
        float time = 0;
        Vector3 startPos = photo.transform.localPosition;
        Vector3 endPos = photo.transform.localPosition + photo.transform.right * 3;
        //endPos.z += 3;

        if (photo != null)
        {
            while (time < timeToShow)
            {
                photo.transform.localPosition = Vector3.Lerp(startPos, endPos, time/timeToShow);

                if(opacity != null && transparentMat != null)
                {
                    Material mat = transparentMat;

                    Color newCol = Color.black;
                    newCol.a = 1 - time / timeToShow;
                    mat.color = newCol;

                    opacity.material = mat;
                }

                time += Time.deltaTime;
                yield return null;
            }
            photo.transform.localPosition = Vector3.Lerp(startPos, endPos, 1);
        }
        else
            yield break;
    }

    Texture2D toTexture2D(RenderTexture rTex)
    {
        Texture2D tex = new Texture2D(512, 512, TextureFormat.RGB24, false);
        // ReadPixels looks at the active RenderTexture.
        RenderTexture.active = rTex;
        tex.ReadPixels(new Rect(0, 0, rTex.width, rTex.height), 0, 0);
        tex.Apply();
        return tex;
    }
}
