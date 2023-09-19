using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public partial class TMP_Typewriter : MonoBehaviour
{

    [SerializeField] private TMP_Text m_textUI = null;


    private string m_parsedText;
    private Action m_onComplete;
    private Tween m_tween;

    private List<string> texts = new List<string> { "how fast can you write?", "can you beat the best score?", "eat, type and repeat", "be type my friend", "remember, you are the best",
    "i passed the 42's piscine, did you?", "write write write", "i forgot to delete this text", "come on, go to play"};


    private void Reset()
    {
        m_textUI = GetComponent<TMP_Text>();
    }


    private void OnDestroy()
    {
        if (m_tween != null)
        {
            m_tween.Kill();
        }

        m_tween = null;
        m_onComplete = null;
    }

    private IEnumerator Start()
    {
        yield return new WaitForSeconds(1);
        Play
        (
        text: texts[UnityEngine.Random.Range(0, texts.Count - 1)],
        speed: UnityEngine.Random.Range(10f, 20f),
        onComplete: () => Start()
        );
    }
    public void Play(string text, float speed, Action onComplete)
    {
        m_textUI.text = text;
        m_onComplete = onComplete;

        m_textUI.ForceMeshUpdate();

        m_parsedText = m_textUI.GetParsedText();

        var length = m_parsedText.Length;
        var duration = 1 / speed * length;

        OnUpdate(0);

        if (m_tween != null)
        {
            m_tween.Kill();
        }

        m_tween = DOTween
            .To(value => OnUpdate(value), 0, 1, duration)
            .SetEase(Ease.Linear)
            .OnComplete(() => StartCoroutine(Start()))
        ;
    }

    public void Skip(bool withCallbacks = true)
    {
        if (m_tween != null)
        {
            m_tween.Kill();
        }

        m_tween = null;

        OnUpdate(1);

        if (!withCallbacks) return;

        if (m_onComplete != null)
        {
            m_onComplete.Invoke();
        }

        m_onComplete = null;
    }


    public void Pause()
    {
        if (m_tween != null)
        {
            m_tween.Pause();
        }
    }

    public void Resume()
    {
        if (m_tween != null)
        {
            m_tween.Play();
        }
    }


    private void OnUpdate(float value)
    {
        var current = Mathf.Lerp(0, m_parsedText.Length, value);
        var count = Mathf.FloorToInt(current);

        m_textUI.maxVisibleCharacters = count;
    }

    private void OnComplete()
    {
        m_tween = null;

        if (m_onComplete != null)
        {
            m_onComplete.Invoke();
        }

        m_onComplete = null;
    }
}
