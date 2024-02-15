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

    private bool hasRunDeselected;

    public override void Start()
    {
        Deselect();

        base.Start();
    }

    public override void Update()
    {
        if (UIManager.Instance.KeyOrControlActive)
        {
            if (UIManager.Instance.CurrentUISelected == position && hasRunDeselected)
            {
                Select();
            }
            else if (UIManager.Instance.CurrentUISelected != position && !hasRunDeselected)
            {
                Deselect();
            }
        }
        else
        {
            if (hovering && hasRunDeselected)
            {
                Select();
            }
            else if (!hovering && !hasRunDeselected)
            {
                Deselect();
            }
        }

        base.Update();
    }

    public void Select()
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

        hasRunDeselected = false;
    }

    public void Deselect()
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

        hasRunDeselected = true;
    }
}
