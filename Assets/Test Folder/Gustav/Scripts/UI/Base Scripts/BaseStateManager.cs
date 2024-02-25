using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public abstract class BaseStateManager : MonoBehaviour
{
    public UI UIInstance { get; set; }
    public Dictionary<string, Color> ColorPairs { get; private set; }
    public Dictionary<string, Vector3> ScalePairs { get; private set; }
    public UIBaseState CurrentState { get; set; }
    public GameObject Pointers { get; protected set; }
    public bool UIActivated { get; set; }

    public virtual void OnStart()
    {
        ColorPairs = new Dictionary<string, Color>();
        ScalePairs = new Dictionary<string, Vector3>();

        SetDictionaries();
    }

    public virtual void OnUpdate()
    {
        CurrentState.UpdateState(this);
    }

    public void SwitchState(UIBaseState state)
    {
        CurrentState.ExitState(this);

        CurrentState = state;

        CurrentState.EnterState(this);
    }

    public void DefaultSelectTransition(float time, GameObject pointers, Transform t = null, Image i = null, TextMeshProUGUI tmp = null)
    {
        pointers.SetActive(true);

        if (t != null)
        {
            TransitionSystem.AddScaleTransition(new ScaleTransition(t, ScalePairs["CurrentObjectSelected"], time, TransitionType.SmoothStart2));
        }
        if (i != null)
        {
            TransitionSystem.AddColorTransition(new ColorTransition(i, ColorPairs["OutlineSelected"], time, TransitionType.SmoothStop2));
        }
        if (tmp != null)
        {
            TransitionSystem.AddColorTransition(new ColorTransition(tmp, ColorPairs["TextSelected"], time, TransitionType.SmoothStop2));
        }
    }

    public void DefaultDeselectTransition(float time, GameObject pointers, Transform t = null, Image i = null, TextMeshProUGUI tmp = null)
    {
        pointers.SetActive(false);

        if (i != null)
        {
            TransitionSystem.AddColorTransition(new ColorTransition(i, ColorPairs["OutlineDeSelected"], time, TransitionType.SmoothStop2));
        }

        if (tmp != null)
        {
            TransitionSystem.AddColorTransition(new ColorTransition(tmp, ColorPairs["TextDeSelected"], time, TransitionType.SmoothStop2));
        }

        if (!UIStateManager.Instance.Transitioning && t != null)
        {
            TransitionSystem.AddScaleTransition(new ScaleTransition(t, Vector3.one, time, TransitionType.SmoothStart2));
        }
    }

    public void SetDictionaries()
    {
        if (!ScalePairs.ContainsKey("CurrentObjectSelected"))
        {
            ScalePairs.Add("CurrentObjectSelected", UIInstance.selectedScale);
        }

        if (!ColorPairs.ContainsKey("OutlineSelected") && !ColorPairs.ContainsKey("TextSelected"))
        {
            ColorPairs.Add("OutlineSelected", UIInstance.outlineSelected);
            ColorPairs.Add("TextSelected", UIInstance.textSelected);
        }

        if (!ColorPairs.ContainsKey("OutlineDeSelected") && !ColorPairs.ContainsKey("TextDeSelected"))
        {
            ColorPairs.Add("OutlineDeSelected", UIInstance.outlineDeselected);
            ColorPairs.Add("TextDeSelected", UIInstance.textDeselected);
        }
    }
}
