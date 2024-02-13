using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class OnLoad : MonoBehaviour
{
    [Header("Drag Image Here")]
    public Image image;
    public Color color;
    public float time = 1.5f;
    public bool fadeIn;

    public void Start()
    {
        if (fadeIn)
        {
            FadeIn(image, time, color);
        }
    }

    private void FadeIn(Image image, float fadeInTIme, Color newColor)
    {
        TransitionSystem.AddColorTransition(new ColorTransition(image, newColor, fadeInTIme, TransitionType.SmoothStart2));
    }
}
