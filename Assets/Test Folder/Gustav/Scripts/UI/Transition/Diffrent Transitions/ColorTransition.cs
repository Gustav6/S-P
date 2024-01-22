using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ColorTransition : Transition
{
    readonly Image image;
    readonly TextMeshProUGUI textMeshPro;
    readonly TransitionType transitionType;
    Color target;
    Color startingColor;

    public ColorTransition(Image _image, Color colorTarget, float timeItTakes, TransitionType _transitionType)
    {
        image = _image;
        startingColor = _image.color;
        target = colorTarget;
        timerMax = timeItTakes;
        transitionType = _transitionType;
    }
    public ColorTransition(TextMeshProUGUI _textMeshPro, Color colorTarget, float timeItTakes, TransitionType _transitionType)
    {
        textMeshPro = _textMeshPro;
        startingColor = textMeshPro.color;
        target = colorTarget;
        timerMax = timeItTakes;
        transitionType = _transitionType;
    }

    public override void Start()
    {
        base.Start();
    }

    public override void Update()
    {
        float t = 0;

        switch (transitionType)
        {
            case TransitionType.SmoothStart2:
                t = TransitionSystem.SmoothStart2(timer / timerMax);
                break;
            case TransitionType.SmoothStart3:
                t = TransitionSystem.SmoothStart3(timer / timerMax);
                break;
            case TransitionType.SmoothStart4:
                t = TransitionSystem.SmoothStart4(timer / timerMax);
                break;
            case TransitionType.SmoothStop2:
                t = TransitionSystem.SmoothStop2(timer / timerMax);
                break;
            case TransitionType.SmoothStop3:
                t = TransitionSystem.SmoothStop3(timer / timerMax);
                break;
            case TransitionType.SmoothStop4:
                t = TransitionSystem.SmoothStop4(timer / timerMax);
                break;
            default:
                break;
        }

        if (image != null)
        {
            image.color = Color.Lerp(startingColor, target, t);
        }
        else
        {
            textMeshPro.color = Color.Lerp(startingColor, target, t);
        }

        base.Update();
    }
}
