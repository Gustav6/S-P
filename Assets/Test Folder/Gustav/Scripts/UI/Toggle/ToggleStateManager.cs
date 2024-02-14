using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ToggleStateManager : BaseStateManager
{
    public ToggleDeselectedState deselectedState = new();
    public ToggleSelectedState selectedState = new();
    public TogglePressedState pressedState = new();

    [HideInInspector] public Image outLineImage;
    [HideInInspector] public Image movingPartImage;
    [HideInInspector] public RectTransform movingPart;
    [HideInInspector] public RectTransform outLine;
    [HideInInspector] public GameObject pointers;

    public float movingPartOffset;

    public override void OnStart()
    {
        base.OnStart();

        outLineImage = transform.GetChild(0).GetComponent<Image>();
        outLine = transform.GetChild(0).GetComponent<RectTransform>();
        movingPart = transform.GetChild(1).GetComponent<RectTransform>();
        movingPartImage = transform.GetChild(1).GetComponent<Image>();
        pointers = transform.GetChild(2).GetComponent<Transform>().gameObject;
        movingPartOffset = movingPart.localPosition.x;

        SetState();

        CurrentState = deselectedState;

        CurrentState.EnterState(this);
    }

    public override void OnUpdate()
    {
        base.OnUpdate();
    }

    public void SetState()
    {
        Toggle toggleInstance = (Toggle)UIInstance;

        if (UIDataManager.instance.toggleValues.ContainsKey(toggleInstance.toggleType))
        {
            toggleInstance.toggleOn = UIDataManager.instance.toggleValues[toggleInstance.toggleType];
        }
        else
        {
            toggleInstance.toggleOn = true;
        }

        if (toggleInstance.toggleOn)
        {
            Vector3 destination = new(movingPartOffset * -1, 0, 0);
            movingPart.localPosition = destination;
        }
        else
        {
            Vector3 destination = new(movingPartOffset, 0, 0);
            movingPart.localPosition = destination;
        }
    }
}
