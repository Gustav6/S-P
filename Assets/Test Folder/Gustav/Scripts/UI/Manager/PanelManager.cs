using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PanelManager : MonoBehaviour
{
    private static Image panelImage;

    void Awake()
    {
        panelImage = GetComponent<Image>();
    }

    public static void FadeIn(float time, Color color, Transition.ExecuteOnCompletion @delegate)
    {
        TransitionSystem.AddColorTransition(new ColorTransition(panelImage, color, time, TransitionType.SmoothStart2, @delegate));
    }

    public static void FadeOut(float time, Color color, Transition.ExecuteOnCompletion @delegate)
    {
        TransitionSystem.AddColorTransition(new ColorTransition(panelImage, color, time, TransitionType.SmoothStop2, @delegate));
    }
}
