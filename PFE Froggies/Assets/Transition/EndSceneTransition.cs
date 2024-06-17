using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting.Antlr3.Runtime;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class EndSceneTransition : MonoBehaviour
{
    public float timeToFade = 0.75f;
    public Image blackScreenTransition;
    public bool unfadeAtStart = true;
    [Space]
    public List<Image> logo = new List<Image>();
    public RectTransform circleToRotate;
    public Vector3 axisToRotate = Vector3.forward;
    public float rotaSpeed = 1;


    private void Start()
    {
        if(blackScreenTransition != null)
        {
            Color color = blackScreenTransition.color;
            color.a = 0;
            blackScreenTransition.color = color;
        }

        if(unfadeAtStart)
        {
            Unfade();
        }
    }

    public void Fade(string targetScene)
    {
        StopAllCoroutines();
        StartCoroutine(CoroutineTransitionFade(targetScene));
    }
    public void Unfade()
    {
        StopAllCoroutines();
        StartCoroutine(CoroutineTransitionUnfade());
    }

    IEnumerator CoroutineTransitionFade(string targetScene)
    {
        float time = 0;
        Color color = blackScreenTransition.color;

        int start = 0;
        int end = 1;

        if(circleToRotate != null)
        {
            circleToRotate.localEulerAngles = Vector3.zero;
        }


        while (time < timeToFade)
        {
            float step = time / timeToFade;
            float alpha = Mathf.Lerp(start, end, step);
            color.a = alpha;
            blackScreenTransition.color = color;

            AlphaImages(alpha);

            yield return null;
            time += Time.deltaTime;

            if (circleToRotate != null)
            {
                Vector3 rota = circleToRotate.localEulerAngles + axisToRotate * rotaSpeed;
                circleToRotate.localEulerAngles = rota;
            }
            
        }
        color.a = end;
        blackScreenTransition.color = color;

        AlphaImages(end);

        SceneManager.LoadScene(targetScene);
    }
    IEnumerator CoroutineTransitionUnfade()
    {
        float time = 0;
        Color color = blackScreenTransition.color;

        int start = 1;
        int end = 0;

        if (circleToRotate != null)
        {
            circleToRotate.localEulerAngles = Vector3.zero;
        }

        while (time < timeToFade)
        {
            float step = time / timeToFade;
            float alpha = Mathf.Lerp(start, end, step);
            color.a = alpha;
            blackScreenTransition.color = color;

            AlphaImages(alpha);

            yield return null;
            time += Time.deltaTime;

            if (circleToRotate != null)
            {
                Vector3 rota = circleToRotate.localEulerAngles + axisToRotate * rotaSpeed;
                circleToRotate.localEulerAngles = rota;
            }
        }
        color.a = end;
        blackScreenTransition.color = color;

        AlphaImages(end);
    }

    void AlphaImages(float alpha)
    {
        for(int i = 0; i < logo.Count; i++)
        {
            Color color = logo[i].color;
            color.a = alpha;
            logo[i].color = color;
        }
    }
}
