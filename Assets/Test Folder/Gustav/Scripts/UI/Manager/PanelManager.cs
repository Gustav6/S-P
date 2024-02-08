using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PanelManager : MonoBehaviour
{
    private static Image panelImage;
    public static Image PanelImage { get { return panelImage; } }

    void Start()
    {
        panelImage = GetComponent<Image>();
        FadeIn(1, new Color(0, 0, 0, 0), UIManager.instance.DisableTransitioning);
    }

    public static void FadeIn(float time, Color color, Transition.ExecuteOnCompletion @delegate)
    {
        TransitionSystem.AddColorTransition(new ColorTransition(PanelImage, color, time, TransitionType.SmoothStart2, @delegate));
    }

    public static void FadeOut(float time, Color color, Transition.ExecuteOnCompletion @delegate)
    {
        TransitionSystem.AddColorTransition(new ColorTransition(PanelImage, color, time, TransitionType.SmoothStop2, @delegate));
    }
}
