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

    [HideInInspector] public UI uI;
    [HideInInspector] public Image outLineImage;
    [HideInInspector] public RectTransform outLinePosition;
    [HideInInspector] public Image sliderImage;
    [HideInInspector] public TextMeshProUGUI text;
    [HideInInspector] public RectTransform sliderPosition;

    public float slidersOffset;
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
        if (uI != null && !uI.UIManagerInstance.Transitioning)
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
        float maxMove = Mathf.Abs(maxMoveValue * 2) * uI.UIManagerInstance.ResolutionScaling;

        float percentage = ((sliderPosition.localPosition.x + Mathf.Abs(slidersOffset)) / maxMove) * uI.UIManagerInstance.ResolutionScaling;

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
