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

    public UI uI;

    public Image outLineImage;
    public Image toggleImage;
    public Image movingPartImage;

    public TextMeshProUGUI text;

    public RectTransform movingPart;
    public RectTransform outLine;

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
        if (uI != null && !uI.Manager.Transitioning)
        {
            currentState.UpdateState(this);
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
            Vector3 destination = new(@switch.movingPartOffset * -1 * @switch.uI.Manager.ResolutionScaling, 0, 0);
            destination += @switch.outLine.position;
            Color newColor = new(0, 0.8f, 0, 1);
            TransitionSystem.AddMoveTransition(new MoveTransition(@switch.movingPart, destination, transitionTime, TransitionType.SmoothStop3, false));
            TransitionSystem.AddColorTransition(new ColorTransition(@switch.toggleImage, newColor, transitionTime, TransitionType.SmoothStop2));
        }
        else if (!@switch.switchOn)
        {
            Vector3 destination = new(@switch.movingPartOffset * @switch.uI.Manager.ResolutionScaling, 0, 0);
            destination += @switch.outLine.position;
            Color newColor = new(0.8f, 0, 0, 1);
            TransitionSystem.AddMoveTransition(new MoveTransition(@switch.movingPart, destination, transitionTime, TransitionType.SmoothStop3, false));
            TransitionSystem.AddColorTransition(new ColorTransition(@switch.toggleImage, newColor, transitionTime, TransitionType.SmoothStop2));
        }

        @switch.uI.activated = false;
    }
}
