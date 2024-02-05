using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

public class ScaleTransition : Transition
{
    Transform transform;
    TransitionType transitionType;
    Vector3 target;
    Vector3 startingScale;
    float pointA;
    float pointB;
    float pointC;

    float amplitude;
    float repetiotions;

    public ScaleTransition(Transform t, Vector3 _target, float totalTime, TransitionType type, ExecuteOnCompletion execute = null, float pA = 0, float pB = 0, float pC = 0)
    {
        transform = t;
        startingScale = transform.localScale;
        target = _target;
        timerMax = totalTime;
        transitionType = type;
        executeOnCompletion += execute;
        pointA = pA;
        pointB = pB;
        pointC = pC;
    }

    public ScaleTransition(Transform t, Vector3 _target, float totalTime, TransitionType type, float _amplitude, float _repetiotions)
    {
        transform = t;
        startingScale = transform.localScale;
        target = _target;
        timerMax = totalTime;
        transitionType = type;
        amplitude = _amplitude;
        repetiotions = _repetiotions;
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
                t = TransitionSystem.SinCurve(repetiotions, amplitude, timer / timerMax);
                break;
            default:
                break;
        }


        if (pointA == 0 && pointB == 0 && pointC == 0)
        {
            if (transform != null)
            {
                transform.localScale = Vector3.Lerp(startingScale, target, t);
            }
        }
        else
        {
            Vector2 temp = startingScale + target * t;
            transform.localScale = temp;
        }

        base.Update();
    }
}
