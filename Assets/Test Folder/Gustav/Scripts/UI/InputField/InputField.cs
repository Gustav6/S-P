using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputField : UI
{
    public InputFieldStateManager InputFieldStateManager { get; private set; }

    [Range(1, 20)] public int maxAmountOfLetters;

    public override void Start()
    {
        InputFieldStateManager = GetComponent<InputFieldStateManager>();

        LoadFunction(InputFieldStateManager);

        base.Start();
    }

    public override void Update()
    {
        UpdateFunction(InputFieldStateManager);

        base.Update();
    }
}
