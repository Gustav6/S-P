using UnityEngine;
using UnityEngine.UI;

public class MoveTransition : Transition
{
    public Transform transform;
    public Vector3 start;
    public Vector3 target;
    public float pointA;
    public float pointB;
    private readonly TransitionType? transitionType;
    private readonly TransitionStart transitionStart;
    private readonly TransitionEnd transitionEnding;
    private readonly bool baseTargetFromObject;

    public MoveTransition(Transform t, Vector3 _target, float totalTime, TransitionType type, bool fromObject = false, ExecuteOnCompletion d = null, float _pointA = 0, float _pointB = 0)
    {
        transform = t;
        start = t.position;
        target = _target;
        timerMax = totalTime;
        transitionType = type;
        baseTargetFromObject = fromObject;
        executeOnCompletion += d;
        pointA = _pointA;
        pointB = _pointB;
    }

    public MoveTransition(Transform t, Vector3 _target, float totalTime, TransitionStart tStart, TransitionEnd tEnding, bool fromObject = false, ExecuteOnCompletion d = null)
    {
        transform = t;
        start = t.position;
        target = _target;
        timerMax = totalTime;
        transitionStart = tStart;
        transitionEnding = tEnding;
        baseTargetFromObject = fromObject;
        executeOnCompletion += d;
    }

    public override void Start()
    {
        base.Start();
    }

    public override void Update()
    {
        float t = 0;

        if (transitionType != null)
        {
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
                case TransitionType.NormalizedBezier3:
                    t = TransitionSystem.NormalizedBezier3(pointA, pointB, timer / timerMax);
                    break;
                default:
                    break;
            }
        }
        else
        {
            float t2 = 0;

            switch (transitionStart)
            {
                case TransitionStart.SmoothStart2:
                    t = TransitionSystem.SmoothStop2(timer / timerMax);
                    break;
                case TransitionStart.SmoothStart3:
                    t = TransitionSystem.SmoothStop3(timer / timerMax);
                    break;
                case TransitionStart.SmoothStart4:
                    t = TransitionSystem.SmoothStop4(timer / timerMax);
                    break;
                default:
                    break;
            }

            switch (transitionEnding)
            {
                case TransitionEnd.SmoothStop2:
                    t2 = TransitionSystem.SmoothStop2(timer / timerMax);
                    break;
                case TransitionEnd.SmoothStop3:
                    t2 = TransitionSystem.SmoothStop3(timer / timerMax);
                    break;
                case TransitionEnd.SmoothStop4:
                    t2 = TransitionSystem.SmoothStop4(timer / timerMax);
                    break;
                default:
                    break;
            }

            t = TransitionSystem.Crossfade(t, t2, timer / timerMax);
        }

        if (transitionType != TransitionType.NormalizedBezier3)
        {
            if (transform != null && !baseTargetFromObject)
            {
                transform.position = Vector3.Lerp(start, target, t);
            }
            else if (transform != null && baseTargetFromObject)
            {
                transform.position = Vector3.Lerp(start, start + target, t);
            }
        }
        else
        {
            Vector2 temp =  start + target * t;
            transform.position = temp;
        }

        base.Update();
    }
}
