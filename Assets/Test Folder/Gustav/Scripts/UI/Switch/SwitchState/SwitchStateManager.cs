using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SwitchStateManager : MonoBehaviour
{
    #region States
    private SwitchBaseState currentState;
    public SwitchSelectedState selectedState = new();
    public SwitchDeselectedState deselectedState = new();
    public SwitchPressedState pressedState = new();
    #endregion

    [HideInInspector] public UI uI;
    [HideInInspector] public Image outLineImage;
    [HideInInspector] public Image toggleImage;
    [HideInInspector] public Image movingPartImage;
    [HideInInspector] public TextMeshProUGUI text;
    [HideInInspector] public RectTransform movingPart;
    [HideInInspector] public RectTransform outLine;

    public float movingPartOffset;
    public bool switchOn;
    public float transitionTime = 0.3f;

    private void Start()
    {
        outLineImage = transform.GetChild(0).GetComponent<Image>();
        outLine = transform.GetChild(0).GetComponent<RectTransform>();
        movingPart = transform.GetChild(1).GetComponent<RectTransform>();
        movingPartImage = transform.GetChild(1).GetComponent<Image>();
        toggleImage = transform.GetChild(1).GetChild(0).GetComponent<Image>();
        text = transform.GetChild(2).GetComponent<TextMeshProUGUI>();
        uI = GetComponent<UI>();

        movingPartOffset = movingPart.localPosition.x;

        currentState = deselectedState;

        currentState.EnterState(this);
    }

    void Update()
    {
        if (uI != null && uI.UIManagerInstance != null)
        {
            if (!UIManager.Transitioning)
            {
                currentState.UpdateState(this);
            }
        }
    }

    public void SwitchState(SwitchBaseState state)
    {
        currentState.ExitState(this);

        currentState = state;

        currentState.EnterState(this);
    }

    public void SwitchOnOff(SwitchStateManager @switch, float transitionTime)
    {
        if (@switch.switchOn)
        {
            Vector3 destination = new(@switch.movingPartOffset * -1 * UIManager.ResolutionScaling, 0, 0);
            destination += @switch.outLine.position;
            Color newColor = new(0, 0.8f, 0, 1);
            TransitionSystem.AddMoveTransition(new MoveTransition(@switch.movingPart, destination, transitionTime, TransitionType.SmoothStop3, false));
            TransitionSystem.AddColorTransition(new ColorTransition(@switch.toggleImage, newColor, transitionTime, TransitionType.SmoothStop2));
        }
        else if (!@switch.switchOn)
        {
            Vector3 destination = new(@switch.movingPartOffset * UIManager.ResolutionScaling, 0, 0);
            destination += @switch.outLine.position;
            Color newColor = new(0.8f, 0, 0, 1);
            TransitionSystem.AddMoveTransition(new MoveTransition(@switch.movingPart, destination, transitionTime, TransitionType.SmoothStop3, false));
            TransitionSystem.AddColorTransition(new ColorTransition(@switch.toggleImage, newColor, transitionTime, TransitionType.SmoothStop2));
        }

        @switch.uI.activated = false;
    }
}
