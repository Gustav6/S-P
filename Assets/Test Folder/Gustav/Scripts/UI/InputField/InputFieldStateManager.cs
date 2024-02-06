using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class InputFieldStateManager : BaseStateManager
{
    public InputFieldDeselectedState deselectedState = new();
    public InputFieldSelectedState selectedState = new();
    public InputFieldPressedState pressedState = new();

    [HideInInspector] public Image image;
    [HideInInspector] public TextMeshProUGUI text;
    [HideInInspector] public GameObject pointers;

    public override void OnStart()
    {
        InputManagerInstance = this;

        base.OnStart();

        image = GetComponentInChildren<Image>();
        text = GetComponentInChildren<TextMeshProUGUI>();
        pointers = transform.GetChild(2).GetComponent<Transform>().gameObject;

        CurrentState = deselectedState;

        CurrentState.EnterState(this);
    }

    public override void OnUpdate()
    {
        base.OnUpdate();
    }
}
