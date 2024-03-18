using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class Selectable : UI
{
    [Header("Will be effected")]
    [SerializeField] private List<Image> images = new();
    [SerializeField] private List<TextMeshProUGUI> texts = new();

    [Header("Selected variables")]
    [SerializeField] private Color imageSelectedColor;
    [SerializeField] private Color textSelectedColor;

    [Header("Deselected variables")]
    [SerializeField] private Color imageDeselectedColor;
    [SerializeField] private Color textDeselectedColor;

    private bool hasRunDeselected;

    public override void Start()
    {
        Deselect();

        base.Start();
    }

    public override void Update()
    {
        if (UIStateManager.Instance.KeyOrControlActive && isInteractable)
        {
            if (UIStateManager.Instance.CurrentUISelected == position && hasRunDeselected)
            {
                Select();
            }
            else if (UIStateManager.Instance.CurrentUISelected != position && !hasRunDeselected)
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
        if (enableOnSelect.Count > 0)
        {
            for (int i = 0; i < enableOnSelect.Count; i++)
            {
                enableOnSelect[i].SetActive(true);
            }
        }

        if (AudioManager.Instance != null)
        {
            AudioManager.Instance.PlaySound("Hover");
        }

        for (int i = 0; i < images.Count; i++)
        {
            TransitionSystem.AddColorTransition(new ColorTransition(images[i], imageSelectedColor, 0.2f, TransitionType.SmoothStop2));
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
        if (enableOnSelect.Count > 0)
        {
            for (int i = 0; i < enableOnSelect.Count; i++)
            {
                enableOnSelect[i].SetActive(false);
            }
        }

        for (int i = 0; i < images.Count; i++)
        {
            TransitionSystem.AddColorTransition(new ColorTransition(images[i], imageDeselectedColor, 0.2f, TransitionType.SmoothStop2));
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
