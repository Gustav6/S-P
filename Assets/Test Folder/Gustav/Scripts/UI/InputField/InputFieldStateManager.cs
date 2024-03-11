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

    [HideInInspector] public Image bgImage;
    [HideInInspector] public GameObject outline;
    [HideInInspector] public TextMeshProUGUI text;

    public override void OnStart()
    {
        base.OnStart();

        bgImage = UIInstance.GetComponentInChildren<Image>();
        text = UIInstance.GetComponentInChildren<TextMeshProUGUI>();
        Pointers = UIInstance.transform.GetChild(2).GetComponent<Transform>().gameObject;

        CurrentState = deselectedState;

        CurrentState.EnterState(this);
    }

    public override void OnUpdate()
    {
        base.OnUpdate();
    }
}
