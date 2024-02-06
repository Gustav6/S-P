using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(UI))]
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
    [HideInInspector] public GameObject pointers;

    public SliderMethods methods;
    public float slidersOffset;
    public float maxMoveValue;
    public float moveDirection;

    void Start()
    {
        outLineImage = transform.GetChild(0).GetComponent<Image>();
        sliderImage = transform.GetChild(1).GetComponent<Image>();
        sliderPosition = transform.GetChild(1).GetComponent<RectTransform>();
        outLinePosition = transform.GetChild(0).GetComponent<RectTransform>();
        maxMoveValue = Mathf.Abs(sliderPosition.localPosition.x);
        text = transform.GetChild(2).GetComponent<TextMeshProUGUI>();
        pointers = transform.GetChild(3).GetComponent<Transform>().gameObject;
        slidersOffset = sliderPosition.localPosition.x;

        methods = GetComponent<SliderMethods>();
        uI = GetComponent<UI>();

        SetStartPositionToValue();

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

    public void SwitchState(SliderBaseState state)
    {
        currentState.ExitState(this);

        currentState = state;

        currentState.EnterState(this);
    }

    public float TotalSlidingPercentage()
    {
        float maxMove = Mathf.Abs(maxMoveValue * 2) * UIManager.ResolutionScaling;

        float percentage = ((sliderPosition.localPosition.x + Mathf.Abs(slidersOffset)) / maxMove) * UIManager.ResolutionScaling;

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
        if (UIManager.DataManagerInstance.sliderValues.ContainsKey(methods.type))
        {
            Vector2 temp = new Vector2(PercentageToPosition(UIManager.DataManagerInstance.sliderValues[methods.type]), sliderPosition.localPosition.y);
            sliderPosition.localPosition = temp;
        }
    }
}
