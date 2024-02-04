using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotationTransition : Transition
{
    static private Transform transform;
    static private Vector3 target;
    private readonly TransitionType? transitionType;
    private float repetitions;
    private float amplitude;

    public RotationTransition(Transform t, Vector3 _target, float totalTime, TransitionType type, float _repetitions = 0, float _amplitude = 0, ExecuteOnCompletion execute = null)
    {
        transform = t;
        target = _target;
        timerMax = totalTime;
        repetitions = _repetitions;
        amplitude = _amplitude;
        transitionType = type;
        executeOnCompletion += execute;
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
            case TransitionType.SinCurve:
                t = TransitionSystem.SinCurve(repetitions, amplitude, timer / timerMax);
                break;
            default:
                break;
        }

        Vector3 temp = target * t;
        transform.Rotate(temp);

        base.Update();
    }
}
