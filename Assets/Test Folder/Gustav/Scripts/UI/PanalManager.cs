using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PanalManager : MonoBehaviour
{
    private static Image panelImage;
    public static Image PanalImage { get { return panelImage; } }

    private Color fadeInColor = new(0, 0, 0, 0);
    private float fadeInTime = 1;

    void Start()
    {
        panelImage = GetComponent<Image>();
        FadeIn();
    }

    public void FadeIn()
    {
        TransitionSystem.AddColorTransition(new ColorTransition(PanalImage, fadeInColor, fadeInTime, TransitionType.SmoothStart2));
    }
}
