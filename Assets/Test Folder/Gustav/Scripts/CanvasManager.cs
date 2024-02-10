using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CanvasManager : MonoBehaviour
{
    private Image logo;
    public Color logoFadeInColor;
    public float logoColorTime = 1f;
    public float panelFadeInTime = 1f;

    void Start()
    {
        for (int i = 0; i < transform.childCount; i++)
        {
            if (transform.GetChild(i).GetComponent<Image>() != null)
            {
                if (transform.GetChild(i).GetComponent<PanelManager>() == null)
                {
                    logo = transform.GetChild(i).GetComponent<Image>();
                }
            }
        }

        NewSceneLoad();
    }

    public void NewSceneLoad()
    {
        Transition.ExecuteOnCompletion execute = null;
        execute += UIManager.instance.DisableTransitioning;

        if (logo != null)
        {
            execute += LoadLogeUponEntering;
        }

        PanelManager.FadeIn(panelFadeInTime, new Color(0, 0, 0, 0), execute);
    }

    public void LoadLogeUponEntering()
    {
        TransitionSystem.AddColorTransition(new ColorTransition(logo, logoFadeInColor, logoColorTime, TransitionType.SmoothStop2));
    }
}
