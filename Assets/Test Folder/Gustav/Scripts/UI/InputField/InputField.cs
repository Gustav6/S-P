using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputField : UI
{
    public InputFieldStateManager InputFieldStateManager { get; private set; }

    public override void Start()
    {
        InputFieldStateManager = GetComponent<InputFieldStateManager>();
        InputFieldStateManager.UIInstance = this;

        base.Start();
    }

    public override void Update()
    {
        if (InputFieldStateManager.UIInstance != null)
        {
            InputFieldStateManager.OnUpdate();
        }

        base.Update();
    }
}
