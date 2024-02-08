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
    public TestAudioManager AudioManagerInstance { get; private set; }
    public bool UIActivated { get; set; }

    public virtual void OnStart()
    {
        ColorPairs = new Dictionary<string, Color>();
        ScalePairs = new Dictionary<string, Vector3>();

        AudioManagerInstance = UIInstance.GetComponentInParent<TestAudioManager>();

        SetDictionaries();
    }

    public virtual void OnUpdate()
    {
        if (!UIManager.instance.Transitioning)
        {
            CurrentState.UpdateState(this);
        }
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

        if (!UIManager.instance.Transitioning && t != null)
        {
            TransitionSystem.AddScaleTransition(new ScaleTransition(t, Vector3.one, time, TransitionType.SmoothStart2));
        }
    }

    public void SetDictionaries()
    {
        if (!ScalePairs.ContainsKey("CurrentObjectSelected"))
        {
            ScalePairs.Add("CurrentObjectSelected", new Vector3(1.025f, 1.025f, 1));
        }

        if (!ColorPairs.ContainsKey("OutlineSelected") && !ColorPairs.ContainsKey("TextSelected"))
        {
            ColorPairs.Add("OutlineSelected", new Color(0, 0, 0, 1));
            ColorPairs.Add("TextSelected", new Color(1, 1, 0, 0.8f));
        }

        if (!ColorPairs.ContainsKey("OutlineDeSelected") && !ColorPairs.ContainsKey("TextDeSelected"))
        {
            ColorPairs.Add("OutlineDeSelected", new Color(0, 0, 0, 0.5f));
            ColorPairs.Add("TextDeSelected", new Color(1, 1, 1, 0.5f));
        }
    }
}