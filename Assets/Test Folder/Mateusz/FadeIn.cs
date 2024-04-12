using JetBrains.Annotations;
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class FadeIn : MonoBehaviour
{
    [SerializeField] private List<GameObject> inOrderOfFadeIn = new();

    [SerializeField] private float fadeDuration;

    public GameObject skipTextGameObject;

    public void Start()
    {
        if (!UIDataManager.instance.hasRunStartScreen)
        {
            UIStateManager.Instance.EnableTransitioning();

            TextMeshProUGUI text = skipTextGameObject.GetComponent<TextMeshProUGUI>();
            TransitionSystem.AddColorTransition(new ColorTransition(text, new Color(1, 1, 1, 1), fadeDuration, TransitionType.SmoothStop2));

            for (int i = 0; i < inOrderOfFadeIn.Count; i++)
            {
                Image imageTemp = inOrderOfFadeIn[i].GetComponent<Image>();
                if (i == inOrderOfFadeIn.Count - 1)
                {
                    TransitionSystem.AddColorTransition(new ColorTransition(imageTemp, new Color(1, 1, 1, 1), fadeDuration, TransitionType.SmoothStop2, FadeOutImages));
                }
                else
                {
                    TransitionSystem.AddColorTransition(new ColorTransition(imageTemp, new Color(1, 1, 1, 1), fadeDuration, TransitionType.SmoothStop2));
                }
            }
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void Update()
    {
        if (Input.GetButtonDown("Jump"))
        {
            UIDataManager.instance.hasRunStartScreen = true;
            UIStateManager.Instance.DisableTransitioning();

            transform.position = new Vector3(transform.position.x + transform.position.x * 2, transform.position.y, 0);
        }
    }

    //IEnumerator DoFadeOut(CanvasGroup canvGroup, float start, float end)
    //{
    //    float elapsedTime = 0F;

    //    while (canvGroup.alpha > 0)
    //    {
    //        elapsedTime += Time.deltaTime;
    //        canvGroup.alpha = Mathf.Lerp(start, end, elapsedTime / fadeDuration);
    //        yield return null;
    //    }

    //    yield return null;
    //}

    public void FadeOutBackground()
    {
        Image imageTemp = GetComponent<Image>();
        TransitionSystem.AddColorTransition(new ColorTransition(imageTemp, new Color(0, 0, 0, 0), fadeDuration / 2, TransitionType.SmoothStart3, EnableHasRunStartScreen));
    }

    public void FadeOutImages()
    {
        for (int i = 0; i < inOrderOfFadeIn.Count; i++)
        {
            Image imageTemp = inOrderOfFadeIn[i].GetComponent<Image>();
            if (i == inOrderOfFadeIn.Count - 1)
            {
                TransitionSystem.AddColorTransition(new ColorTransition(imageTemp, new Color(1, 1, 1, 0), fadeDuration, TransitionType.SmoothStop2, FadeOutBackground));
            }
            else
            {
                TransitionSystem.AddColorTransition(new ColorTransition(imageTemp, new Color(1, 1, 1, 0), fadeDuration, TransitionType.SmoothStop2));
            }
        }

        TextMeshProUGUI text = skipTextGameObject.GetComponent<TextMeshProUGUI>();
        TransitionSystem.AddColorTransition(new ColorTransition(text, new Color(1, 1, 1, 0), fadeDuration, TransitionType.SmoothStop2));
    }

    private void EnableHasRunStartScreen()
    {
        UIDataManager.instance.hasRunStartScreen = true;
        UIStateManager.Instance.DisableTransitioning();
        Destroy(gameObject);
    }
}
