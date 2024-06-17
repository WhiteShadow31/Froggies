using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class EndSceneTransition : MonoBehaviour
{
    public float timeToFade = 0.75f;
    public Image blackScreenTransition;


    private void Start()
    {
        if(blackScreenTransition != null)
        {
            Color color = blackScreenTransition.color;
            color.a = 0;
            blackScreenTransition.color = color;
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

        while (time < timeToFade)
        {
            float step = time / timeToFade;
            float alpha = Mathf.Lerp(start, end, step);
            color.a = alpha;
            blackScreenTransition.color = color;

            yield return null;
            time += Time.deltaTime;
        }
        color.a = end;
        blackScreenTransition.color = color;

        SceneManager.LoadScene(targetScene);
    }
    IEnumerator CoroutineTransitionUnfade()
    {
        float time = 0;
        Color color = blackScreenTransition.color;

        int start = 1;
        int end = 0;

        while (time < timeToFade)
        {
            float step = time / timeToFade;
            float alpha = Mathf.Lerp(start, end, step);
            color.a = alpha;
            blackScreenTransition.color = color;

            yield return null;
            time += Time.deltaTime;
        }
        color.a = end;
        blackScreenTransition.color = color;
    }
}
