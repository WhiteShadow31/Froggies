using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MultipleDialogue : Dialogue
{
    [Header("Following Sprites")]
    public List<Sprite> followingSprites = new List<Sprite>();
    public float timeToChange = 1.5f;
    bool _finished = false;
    bool _isShowingSprites = false;

    IEnumerator CoroutineNextSprite()
    {
        _isShowingSprites = true;
        if(_bubbleImage != null)
        {
            for(int i = 0; i < followingSprites.Count; i++)
            {
                yield return new WaitForSeconds(timeToChange);
                _bubbleImage.sprite = followingSprites[i];
            }
            yield return new WaitForSeconds(timeToChange);
            _bubbleImage.gameObject.SetActive(false);
        }
        _isShowingSprites = false;
    }

    protected override void FixedUpdate()
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

                if(_isShowingSprites ==false)
                {
                    StartCoroutine(CoroutineNextSprite());
                }

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
}
