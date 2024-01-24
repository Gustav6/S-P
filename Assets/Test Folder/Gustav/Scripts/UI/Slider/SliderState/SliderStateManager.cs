using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SliderStateManager : MonoBehaviour
{
    #region States
    public SliderBaseState currentState;
    public SliderSelectedState selectedState = new();
    public SliderDeselectedState deselectedState = new();
    public SliderPressedState pressedState = new();
    #endregion

    public UI uI;

    public Image outLineImage;
    public RectTransform outLinePosition;

    public RectTransform sliderPosition;
    public Image sliderImage;
    public float slidersOffset;

    public TextMeshProUGUI text;

    public float maxMoveValue;
    public float moveDirection;

    void Start()
    {
        outLineImage = transform.GetChild(0).GetComponent<Image>();
        sliderImage = transform.GetChild(1).GetComponent<Image>();
        uI = GetComponent<UI>();
        sliderPosition = transform.GetChild(1).GetComponent<RectTransform>();
        outLinePosition = transform.GetChild(0).GetComponent<RectTransform>();
        maxMoveValue = Mathf.Abs(sliderPosition.localPosition.x);
        text = transform.GetChild(2).GetComponent<TextMeshProUGUI>();
        slidersOffset = sliderPosition.localPosition.x;

        currentState = deselectedState;

        currentState.EnterState(this);  
    }

    void Update()
    {
        if (uI != null && !uI.Manager.Transitioning)
        {
            currentState.UpdateState(this);
        }
    }

    public void SwitchState(SliderBaseState state)
    {
        currentState.ExitState(this);

        currentState = state;

        currentState.EnterState(this);
    }

    public float TotalSlidingPercentage()
    {
        float maxMove = Mathf.Abs(maxMoveValue * 2) * uI.Manager.ResolutionScaling;

        float percentage = ((sliderPosition.localPosition.x + Mathf.Abs(slidersOffset)) / maxMove) * uI.Manager.ResolutionScaling;

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
}
