using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PauseController : MonoBehaviour
{
    // This class and things related to it are unused.
    public event EventHandler<OnPausedEventArgs> OnPaused;

    private bool _isPaused;

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
            TogglePause();
    }

    private void TogglePause()
    {
        _isPaused = !_isPaused;

        OnPaused?.Invoke(this, new OnPausedEventArgs { isPaused = _isPaused });

        // TODO: Spawn PauseUI.
    }
}

public class OnPausedEventArgs : EventArgs
{
    internal bool isPaused;
}
