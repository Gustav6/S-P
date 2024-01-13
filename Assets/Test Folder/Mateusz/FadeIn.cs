using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FadeIn : MonoBehaviour
{

    private bool screenFaded = false;

    public float fadeDuration = 0.4f;

    

    public void Start()
    {
        var canvGroup = GetComponent<CanvasGroup>();
        StartCoroutine(DoFadeOut(canvGroup, canvGroup.alpha, screenFaded ? 1 : 0));
    }

    public void Update()
    {
        if (Input.GetButtonDown("Jump"))
        {
            fadeDuration = 0.7f;
        }
    }

    IEnumerator DoFadeOut(CanvasGroup canvGroup, float start, float end)
    {
        float elapsedTime = 0F;

        while (canvGroup.alpha > 0)
        {
            elapsedTime += Time.deltaTime;
            canvGroup.alpha = Mathf.Lerp(start, end, elapsedTime / fadeDuration);
            yield return null;
        }

        yield return null;
    }
}
