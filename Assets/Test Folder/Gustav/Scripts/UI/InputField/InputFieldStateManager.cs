using NUnit.Framework.Internal;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class InputFieldStateManager : MonoBehaviour
{
    #region States
    private InputFieldBaseState currentState;

    public InputFieldSelectedState selectedState = new();
    public InputFieldDeselectedState deselectedState = new();
    public InputFieldPressedState pressedState = new();
    #endregion

    [HideInInspector] public UI uI;
    [HideInInspector] public Image image;
    [HideInInspector] public TextMeshProUGUI text;
    [HideInInspector] public GameObject pointers;

    void Start()
    {
        uI = GetComponent<UI>();
        image = GetComponentInChildren<Image>();
        text = GetComponentInChildren<TextMeshProUGUI>();
        pointers = transform.GetChild(2).GetComponent<Transform>().gameObject;

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

    public void SwitchState(InputFieldBaseState state)
    {
        currentState.ExitState(this);

        currentState = state;

        currentState.EnterState(this);
    }
}
