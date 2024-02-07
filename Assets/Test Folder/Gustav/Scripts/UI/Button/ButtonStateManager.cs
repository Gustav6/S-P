using NUnit.Framework.Internal;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ButtonStateManager : BaseStateManager
{
    public ButtonDeselectedState deselectedState = new();
    public ButtonSelectedState selectedState = new();
    public ButtonPressedState pressedState = new();

    [HideInInspector] public Image outlineImage;
    [HideInInspector] public TextMeshProUGUI text;
    [HideInInspector] public GameObject pointers;

    public override void OnStart()
    {
        ButtonManagerInstance = this;

        base.OnStart();

        outlineImage = UIInstance.GetComponentInChildren<Image>();
        pointers = UIInstance.transform.GetChild(2).GetComponent<Transform>().gameObject;
        text = UIInstance.GetComponentInChildren<TextMeshProUGUI>();

        CurrentState = deselectedState;

        CurrentState.EnterState(this);  
    }

    public override void OnUpdate()
    {
        base.OnUpdate();
    }
}