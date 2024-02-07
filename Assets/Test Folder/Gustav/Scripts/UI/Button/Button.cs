using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(ButtonStateManager))]
public class Button : UI
{
    public ButtonStateManager ButtonStateManager { get; private set; }

    public bool transitionToScene;
    public NewScene NewScene;

    public bool transitionToPrefab;
    public bool prefabScaleTransition;
    public bool prefabMoveTransition;
    public GameObject prefab;

    public override void Start()
    {
        ButtonStateManager = GetComponent<ButtonStateManager>();
        ButtonStateManager.UIInstance = this;

        if (ButtonStateManager.UIInstance != null)
        {
            ButtonStateManager.OnStart();
        }

        base.Start();
    }

    public override void Update()
    {
        if (ButtonStateManager.UIInstance != null)
        {
            ButtonStateManager.OnUpdate();
        }

        base.Update();
    }
}

public enum NewScene
{
    Game,
    Menu,
}
