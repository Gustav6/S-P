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

    [HideInInspector] public Image bgImage;
    [HideInInspector] public RectTransform movingPart;
    [HideInInspector] public TextMeshProUGUI textForV2;
    [HideInInspector] public GameObject checkMark;

    public float movingPartOffset;

    private Toggle referenceScript;

    public override void OnStart()
    {
        base.OnStart();

        referenceScript = (Toggle)UIInstance;

        bgImage = transform.GetChild(0).GetComponent<Image>();

        if (referenceScript.version == ToggleVersion.Version1)
        {
            movingPart = transform.GetChild(2).GetComponent<RectTransform>();
            movingPartOffset = movingPart.localPosition.x;
        }
        else if (referenceScript.version == ToggleVersion.Version2)
        {
            textForV2 = transform.GetChild(1).GetComponent<TextMeshProUGUI>();
            checkMark = transform.GetChild(3).gameObject;
            referenceScript.toggleOn = true;
        }

        if (referenceScript.version == ToggleVersion.Version1)
        {
            SetState();
        }

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
