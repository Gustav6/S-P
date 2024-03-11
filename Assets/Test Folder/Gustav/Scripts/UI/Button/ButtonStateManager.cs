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

    [HideInInspector] public Image backgroundImage;
    [HideInInspector] public GameObject outline;
    [HideInInspector] public TextMeshProUGUI text;

    public override void OnStart()
    {
        base.OnStart();

        backgroundImage = UIInstance.GetComponentInChildren<Image>();
        outline = UIInstance.transform.GetChild(3).gameObject;
        Pointers = UIInstance.transform.GetChild(2).GetComponent<Transform>().gameObject;
        text = UIInstance.GetComponentInChildren<TextMeshProUGUI>();

        CurrentState = deselectedState;

        CurrentState.EnterState(this);  
    }

    public override void OnUpdate()
    {
        base.OnUpdate();
    }
}