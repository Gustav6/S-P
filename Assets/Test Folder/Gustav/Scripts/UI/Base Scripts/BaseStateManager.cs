using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public abstract class BaseStateManager : MonoBehaviour
{
    public UI UIInstance { get; set; }
    public Dictionary<string, Color> ColorPairs { get; private set; }
    public Dictionary<string, Vector3> ScalePairs { get; private set; }
    public UIBaseState CurrentState { get; set; }
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

    public void DefaultSelectTransition(float time, List<GameObject> enable = null, Image bg = null, Transform t = null, TextMeshProUGUI tmp = null)
    {
        if (enable != null && enable.Count > 0)
        {
            for (int i = 0; i < enable.Count; i++)
            {
                enable[i].SetActive(true);
            }
        }

        if (t != null)
        {
            TransitionSystem.AddScaleTransition(new ScaleTransition(t, ScalePairs["CurrentObjectSelected"], time, TransitionType.SmoothStart2));
        }

        if (bg != null)
        {
            TransitionSystem.AddColorTransition(new ColorTransition(bg, ColorPairs["BgSelected"], time, TransitionType.SmoothStop2));
        }

        if (tmp != null)
        {
            TransitionSystem.AddColorTransition(new ColorTransition(tmp, ColorPairs["TextSelected"], 0.1f, TransitionType.SmoothStop2));
        }
    }

    public void DefaultDeselectTransition(float time, List<GameObject> disable = null, Image bg = null, Transform t = null, TextMeshProUGUI tmp = null)
    {
        if (disable != null && disable.Count > 0)
        {
            for (int i = 0; i < disable.Count; i++)
            {
                disable[i].SetActive(false);
            }
        }

        if (bg != null)
        {
            TransitionSystem.AddColorTransition(new ColorTransition(bg, ColorPairs["BgDeselected"], time, TransitionType.SmoothStop2));
        }

        if (tmp != null)
        {
            TransitionSystem.AddColorTransition(new ColorTransition(tmp, ColorPairs["TextDeselected"], 0.1f, TransitionType.SmoothStop2));
        }

        if (t != null)
        {
            TransitionSystem.AddScaleTransition(new ScaleTransition(t, Vector3.one, time, TransitionType.SmoothStart2));
        }
    }

    public void SetDictionaries()
    {
        ScalePairs.TryAdd("CurrentObjectSelected", UIInstance.selectedScale);
        ColorPairs.TryAdd("BgSelected", UIInstance.bgSelected);
        ColorPairs.TryAdd("TextSelected", UIInstance.textSelected);
        ColorPairs.TryAdd("BgDeselected", UIInstance.bgDeselected);
        ColorPairs.TryAdd("TextDeselected", UIInstance.textDeselected);
    }
}
