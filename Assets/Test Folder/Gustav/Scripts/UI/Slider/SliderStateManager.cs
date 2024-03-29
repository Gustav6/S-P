using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SliderStateManager : BaseStateManager
{
    public SliderDeselectedState deselectedState = new();
    public SliderSelectedState selectedState = new();
    public SliderPressedState pressedState = new();

    [HideInInspector] public Image bgImage;
    [HideInInspector] public RectTransform outLinePosition;
    [HideInInspector] public Image sliderImage;
    [HideInInspector] public TextMeshProUGUI text;
    [HideInInspector] public RectTransform sliderPosition;

    [HideInInspector] public float slidersOffset;
    [HideInInspector] public float maxMoveValue;
    [HideInInspector] public float moveDirection;

    public override void OnStart()
    {
        base.OnStart();

        bgImage = transform.GetChild(0).GetComponent<Image>();
        outLinePosition = transform.GetChild(0).GetComponent<RectTransform>();
        sliderImage = transform.GetChild(2).GetComponent<Image>();
        sliderPosition = transform.GetChild(2).GetComponent<RectTransform>();
        maxMoveValue = Mathf.Abs(sliderPosition.localPosition.x);
        text = transform.GetChild(3).GetComponent<TextMeshProUGUI>();
        slidersOffset = sliderPosition.localPosition.x;

        SetStartPositionToValue();

        CurrentState = deselectedState;

        CurrentState.EnterState(this);
    }

    public override void OnUpdate()
    {
        base.OnUpdate();
    }

    public float TotalSlidingPercentage()
    {
        float maxMove = Mathf.Abs(maxMoveValue * 2) * UIStateManager.Instance.ResolutionScaling;

        float percentage = ((sliderPosition.localPosition.x + Mathf.Abs(slidersOffset)) / maxMove) * UIStateManager.Instance.ResolutionScaling;

        if (percentage > 0.99f)
        {
            return 1;
        }
        else if (percentage < 0.01f)
        {
            return 0;
        }
        else
        {
            return percentage;
        }
    }

    public float PercentageToPosition(float value)
    {
        return value * (maxMoveValue * 2) - maxMoveValue;
    }

    public void SetStartPositionToValue()
    {
        Slider sliderInstance = (Slider)UIInstance;

        if (UIDataManager.instance.sliderValues.ContainsKey(sliderInstance.sliderType))
        {
            Vector2 temp = new(PercentageToPosition(UIDataManager.instance.sliderValues[sliderInstance.sliderType]), sliderPosition.localPosition.y);
            sliderPosition.localPosition = temp;
        }
    }
}
