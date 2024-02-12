using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CanvasManager : MonoBehaviour
{
    public Image logo;
    public Color logoFadeInColor;
    public float logoColorTime = 1.5f;
    public float panelFadeInTime = 1;
    public float moveUITime = 1;

    void Start()
    {
        if (logo != null)
        {
            LoadLogeUponEntering();
        }

        MoveInUI();

        NewSceneLoad();
    }

    public void NewSceneLoad()
    {
        PanelManager.FadeIn(panelFadeInTime, new Color(0, 0, 0, 0), null);
    }

    public void LoadLogeUponEntering()
    {
        TransitionSystem.AddColorTransition(new ColorTransition(logo, logoFadeInColor, logoColorTime, TransitionType.SmoothStart2));
    }

    public void MoveInUI()
    {
        Transition.ExecuteOnCompletion execute = null;
        execute += UIManager.instance.DisableTransitioning;

        //UIManager.instance.MoveUIToDestionation(moveUITime, new Vector3(-1000, 0, 0), new Vector3(1000, 0, 0), true, true, execute);
        UIManager.instance.MoveUIToStart(moveUITime, execute);
    }
}
