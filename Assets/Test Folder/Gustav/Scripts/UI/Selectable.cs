using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Selectable : UI
{
    [Header("Will be effected")]
    [SerializeField] private List<Image> images = new();
    [SerializeField] private List<TextMeshProUGUI> texts = new();

    [Header("Selected variables")]
    [SerializeField] private Color textSelectedColor;

    [Header("Deselected variables")]
    [SerializeField] private float imageDeselectedAlpha;
    [SerializeField] private Color textDeselectedColor;

    private bool hasRunDeslected;

    public override void Start()
    {
        base.Start();
    }

    public override void Update()
    {
        if (hovering && hasRunDeslected)
        {
            OnSelect();
        }
        else if (!hovering && !hasRunDeslected)
        {
            OnDeselect();
        }

        base.Update();
    }

    public void OnSelect()
    {
        for (int i = 0; i < images.Count; i++)
        {
            Color temp = images[i].color;
            temp.a = 1;

            TransitionSystem.AddColorTransition(new ColorTransition(images[i], temp, 0.2f, TransitionType.SmoothStop2));
        }

        for (int i = 0; i < texts.Count; i++)
        {
            TransitionSystem.AddColorTransition(new ColorTransition(texts[i], textSelectedColor, 0.2f, TransitionType.SmoothStop2));
        }

        Vector3 scale = new(1, 1);

        TransitionSystem.AddScaleTransition(new ScaleTransition(transform, scale, 0.2f, TransitionType.SmoothStop2));

        hasRunDeslected = false;
    }

    public void OnDeselect()
    {
        for (int i = 0; i < images.Count; i++)
        {
            Color temp = images[i].color;
            temp.a = imageDeselectedAlpha;

            TransitionSystem.AddColorTransition(new ColorTransition(images[i], temp, 0.2f, TransitionType.SmoothStop2));
        }

        for (int i = 0; i < texts.Count; i++)
        {
            TransitionSystem.AddColorTransition(new ColorTransition(texts[i], textDeselectedColor, 0.2f, TransitionType.SmoothStop2));
        }

        Vector3 scale = new(0.95f, 0.95f);

        TransitionSystem.AddScaleTransition(new ScaleTransition(transform, scale, 0.2f, TransitionType.SmoothStop2));

        hasRunDeslected = true;
    }
}
