using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class OnLoad : MonoBehaviour
{
    public Color logoFadeInColor;
    public float logoColorTime = 1.5f;
    public bool fadeIn;

    public void Start()
    {
        if (fadeIn)
        {
            FadeIn(gameObject.GetComponent<Image>(), logoColorTime, logoFadeInColor);
        }
    }

    public void FadeIn(Image image, float fadeInTIme, Color newColor)
    {
        TransitionSystem.AddColorTransition(new ColorTransition(image, newColor, fadeInTIme, TransitionType.SmoothStart2));
    }
}
