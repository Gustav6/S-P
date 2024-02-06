using NUnit.Framework.Internal;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(UI))]
public class ButtonStateManager : MonoBehaviour
{
    #region States
    private ButtonBaseState currentState;

    public ButtenSelectedState selectedState = new();
    public ButtenPressedState pressedState = new();
    public ButtenDeselectedState deselectedState = new();
    #endregion

    [HideInInspector] public UI uI;
    [HideInInspector] public Image outlineImage;
    [HideInInspector] public TextMeshProUGUI text;
    [HideInInspector] public GameObject pointers;

    public ButtonMethods methods;

    void Start()
    {
        outlineImage = GetComponentInChildren<Image>();
        text = GetComponentInChildren<TextMeshProUGUI>();
        uI = GetComponent<UI>();
        methods = GetComponent<ButtonMethods>();
        pointers = transform.GetChild(2).GetComponent<Transform>().gameObject;

        uI.SetRefrences();

        currentState = deselectedState;

        currentState.EnterState(this);
    }

    void Update()
    {
        if (uI.UIManagerInstance != null && !UIManager.Transitioning)
        {
            currentState.UpdateState(this);
        }
    }

    public void SwitchState(ButtonBaseState state)
    {
        currentState.ExitState(this);

        currentState = state;

        currentState.EnterState(this);
    }
}