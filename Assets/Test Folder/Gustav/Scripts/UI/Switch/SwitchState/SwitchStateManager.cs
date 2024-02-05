using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(UI))]
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
    [HideInInspector] public Transform pointers;

    public SwitchMethods methods;
    public Color offColor = new(0, 0.8f, 0, 1);
    public Color onColor = new(0.8f, 0, 0, 1);

    public bool switchOn;
    [Range(0.1f, 1)] public float transitionTime = 0.3f;

    public float movingPartOffset;

    private void Start()
    {
        outLineImage = transform.GetChild(0).GetComponent<Image>();
        outLine = transform.GetChild(0).GetComponent<RectTransform>();
        movingPart = transform.GetChild(1).GetComponent<RectTransform>();
        movingPartImage = transform.GetChild(1).GetComponent<Image>();
        toggleImage = transform.GetChild(1).GetChild(0).GetComponent<Image>();
        text = transform.GetChild(2).GetComponent<TextMeshProUGUI>();
        pointers = transform.GetChild(3).GetComponent<Transform>();
        movingPartOffset = movingPart.localPosition.x;

        uI = GetComponent<UI>();
        methods = GetComponent<SwitchMethods>();

        SetState();

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

    public void SwitchState(SwitchBaseState state)
    {
        currentState.ExitState(this);

        currentState = state;

        currentState.EnterState(this);
    }

    public void SetState()
    {
        if (UIManager.DataManagerInstance.switchValues.ContainsKey(methods.type))
        {
            switchOn = UIManager.DataManagerInstance.switchValues[methods.type];
        }
        else
        {
            switchOn = true;
        }

        if (switchOn)
        {
            Vector3 destination = new(movingPartOffset * -1, 0, 0);
            movingPart.localPosition = destination;
            toggleImage.color = onColor;
        }
        else
        {
            Vector3 destination = new(movingPartOffset, 0, 0);
            movingPart.localPosition = destination;
            toggleImage.color = offColor;
        }
    }
}