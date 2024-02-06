using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Toggle : UI
{
    public ToggleStateManager ToggleStateManager { get; private set; }

    public override void Start()
    {
        ToggleStateManager = GetComponent<ToggleStateManager>();
        ToggleStateManager.UIInstance = this;

        if (ToggleStateManager.UIInstance != null)
        {
            ToggleStateManager.OnStart();
        }

        base.Start();
    }

    public override void Update()
    {
        if (ToggleStateManager.UIInstance != null)
        {
            ToggleStateManager.OnUpdate();
        }

        base.Update();
    }
}
