using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FPSCounter : MonoBehaviour
{
    private float _deltaTime, _fps;
    private bool _shouldCountFPS;

    
    private void Start()
    {
        ToggleVSync();
        ToggleFPSCounter();
    }

    /// <summary>
    /// Toggles custom VSync off and on.
    /// Unity VSync seems to have some issues.
    /// </summary>
    // refreshRate is obsolete, but it is useful in this case.
    [System.Obsolete]
    public void ToggleVSync()
    {
        if (Application.targetFrameRate == -1)
            Application.targetFrameRate = Screen.currentResolution.refreshRate;
        else
        {
            Application.targetFrameRate = -1;
        }
    }

    /// <summary>
    /// Sets Unity's target framerate.
    /// </summary>
    public void SetTargetFramerate(int target)
    {
        Application.targetFrameRate = target;
    }

    public void ToggleFPSCounter()
    {
        _shouldCountFPS = true;
    }

    private void Update()
    {
        if (_shouldCountFPS)
            _deltaTime += (Time.deltaTime - _deltaTime) * 0.1f;
    }

    private void OnGUI()
    {
        if (!_shouldCountFPS)
            return;

        int w = Screen.width, h = Screen.height;

        GUIStyle style = new GUIStyle();

        Rect rect = new Rect(0, 0, w, h * 2 / 100);
        style.alignment = TextAnchor.UpperLeft;
        style.fontSize = h * 2 / 50;
        style.normal.textColor = new Color(0.0f, 0.0f, 0.5f, 1.0f);

        _fps = 1.0f / _deltaTime;
        string text = string.Format("{0:0.} FPS", _fps);
        GUI.Label(rect, text, style);
    }
}
