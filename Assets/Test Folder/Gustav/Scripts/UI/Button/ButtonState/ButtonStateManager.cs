using NUnit.Framework.Internal;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ButtonStateManager : MonoBehaviour
{
    #region States
    private ButtonBaseState currentState;

    public ButtenSelectedState selectedState = new();
    public ButtenPressedState pressedState = new();
    public ButtenDeselectedState deselectedState = new();
    #endregion

    public UI uI;
    public Image image;
    public TextMeshProUGUI text;

    void Start()
    {
        image = GetComponentInChildren<Image>();
        text = GetComponentInChildren<TextMeshProUGUI>();
        uI = GetComponent<UI>();

        currentState = deselectedState;

        currentState.EnterState(this);
    }

    void Update()
    {
        currentState.UpdateState(this);
    }

    public void SwitchState(ButtonBaseState state)
    {
        currentState.ExitState(this);

        currentState = state;

        currentState.EnterState(this);
    }
}