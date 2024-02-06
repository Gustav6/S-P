using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UI : MonoBehaviour
{
    public Dictionary<string, Color> colorPairs = new();
    public Dictionary<string, Vector3> scalePairs = new();

    public UIManager UIManagerInstance { get; private set; }
    public AudioManager AudioManagerInstance { get; private set; }

    public bool activated = false;
    public Vector2 position;

    void Start()
    {
        AudioManagerInstance = GetComponentInParent<AudioManager>();
        UIManagerInstance = GetComponentInParent<UIManager>();
    }

    public void DefaultSelectTransition(float time, GameObject pointers, Transform t = null, Image i = null, TextMeshProUGUI tmp = null)
    {
        pointers.SetActive(true);

        if (t != null)
        {
            TransitionSystem.AddScaleTransition(new ScaleTransition(t, scalePairs["CurrentObjectSelected"], time, TransitionType.SmoothStart2));
        }
        if (i != null)
        {
            TransitionSystem.AddColorTransition(new ColorTransition(i, colorPairs["OutlineSelected"], time, TransitionType.SmoothStop2));
        }
        if (tmp != null)
        {
            TransitionSystem.AddColorTransition(new ColorTransition(tmp, colorPairs["TextSelected"], time, TransitionType.SmoothStop2));
        }
    }

    public void DefaultDeselectTransition(float time, GameObject pointers, Transform t = null, Image i = null, TextMeshProUGUI tmp = null)
    {
        pointers.SetActive(false);

        TransitionSystem.AddColorTransition(new ColorTransition(i, colorPairs["OutlineDeSelected"], time, TransitionType.SmoothStop2));
        TransitionSystem.AddColorTransition(new ColorTransition(tmp, colorPairs["TextDeSelected"], time, TransitionType.SmoothStop2));

        if (!UIManager.Transitioning && t != null)
        {
            TransitionSystem.AddScaleTransition(new ScaleTransition(t, Vector3.one, time, TransitionType.SmoothStart2));
        }
    }

    public void SetRefrences()
    {
        if (!scalePairs.ContainsKey("CurrentObjectSelected"))
        {
            scalePairs.Add("CurrentObjectSelected", new Vector3(1.025f, 1.025f, 1));
        }

        if (!colorPairs.ContainsKey("OutlineSelected") && !colorPairs.ContainsKey("TextSelected"))
        {
            colorPairs.Add("OutlineSelected", new Color(0, 0, 0, 1));
            colorPairs.Add("TextSelected", new Color(1, 1, 0, 0.8f));
        }

        if (!colorPairs.ContainsKey("OutlineDeSelected") && !colorPairs.ContainsKey("TextDeSelected"))
        {
            colorPairs.Add("OutlineDeSelected", new Color(0, 0, 0, 0.5f));
            colorPairs.Add("TextDeSelected", new Color(1, 1, 1, 0.5f));
        }
    }
}
