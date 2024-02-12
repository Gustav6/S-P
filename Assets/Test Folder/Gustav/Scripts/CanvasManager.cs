using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CanvasManager : MonoBehaviour
{
    private Image logo;
    public Color logoFadeInColor;
    public float logoColorTime = 1.5f;
    public float panelFadeInTime = 1;
    public float moveUITime = 2;

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

        if (logo != null)
        {
            LoadLogeUponEntering();
        }

        MoveInUI();

        NewSceneLoad();
    }

    public void NewSceneLoad()
    {
        Transition.ExecuteOnCompletion execute = null;
        execute += UIManager.instance.DisableTransitioning;

        PanelManager.FadeIn(panelFadeInTime, new Color(0, 0, 0, 0), execute);
    }

    public void LoadLogeUponEntering()
    {
        TransitionSystem.AddColorTransition(new ColorTransition(logo, logoFadeInColor, logoColorTime, TransitionType.SmoothStart2));
    }

    public void MoveInUI()
    {
        UIManager.instance.MoveUIToDestionation(moveUITime, new Vector3(-1000, 0, 0), new Vector3(1000, 0, 0), true);
    }
}
