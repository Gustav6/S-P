using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PanelManager : MonoBehaviour
{
    private static Image panelImage;
    public static Image PanelImage { get { return panelImage; } }

    private Color fadeInColor = new(0, 0, 0, 0);
    private float fadeInTime = 1;

    void Start()
    {
        panelImage = GetComponent<Image>();
        FadeIn();
    }

    public void FadeIn()
    {
        Transition.ExecuteOnCompletion @delegate = null;
        @delegate += UIManager.DisableTransitioning;
        TransitionSystem.AddColorTransition(new ColorTransition(PanelImage, fadeInColor, fadeInTime, TransitionType.SmoothStart2, @delegate));
    }
}
