using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PanelManager : MonoBehaviour
{
    private static Image panelImage;
    public OnLoad OnLoadInstance { get; private set; }
    public static PanelManager Instance { get; private set; }

    void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
            Destroy(this);

        OnLoadInstance = GetComponent<OnLoad>();
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
