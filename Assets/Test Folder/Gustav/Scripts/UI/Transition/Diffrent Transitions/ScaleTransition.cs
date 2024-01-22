using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class ScaleTransition : Transition
{
    Transform transform;
    TransitionType transitionType;
    Vector3 target;
    Vector3 startingScale;

    public ScaleTransition(Transform _transform, Vector3 scaleTarget, float timeItTakes, TransitionType _transitionType)
    {
        transform = _transform;
        startingScale = transform.localScale;
        target = scaleTarget;
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

        if (transform != null)
        {
            transform.localScale = Vector3.Lerp(startingScale, target, t);
        }

        base.Update();
    }
}
