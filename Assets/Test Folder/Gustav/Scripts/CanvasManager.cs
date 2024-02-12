using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CanvasManager : MonoBehaviour
{
    public float panelFadeInTime = 1;
    public float moveUITime = 1;

    void Start()
    {
        MoveInUI();

        NewSceneLoad();
    }

    public void NewSceneLoad()
    {
        PanelManager.FadeIn(panelFadeInTime, new Color(0, 0, 0, 0), null);
    }

    public void MoveInUI()
    {
        Transition.ExecuteOnCompletion execute = null;
        execute += UIManager.instance.DisableTransitioning;

        Vector3 startPosition = new(-Screen.width, 0);

        UIManager.instance.CurrentUIPrefab.transform.localPosition = startPosition;
        UIManager.instance.MoveUIToStart(moveUITime, execute);
    }
}
