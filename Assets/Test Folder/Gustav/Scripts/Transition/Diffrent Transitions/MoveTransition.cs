using UnityEngine;
using UnityEngine.UI;

public class MoveTransition : Transition
{
    public Transform transform;
    public Vector3 start;
    public Vector3 target;
    public float windUp;
    public float overShoot;

    private readonly TransitionType? transitionType;
    private readonly TransitionType? transitionStart;
    private readonly TransitionType? transitionEnding;
    private readonly bool baseTargetFromObject;
    private Vector3 referenceStartingPosition;

    public MoveTransition(Transform t, Vector3 _target, float totalTime, TransitionType type, bool fromObject = false, float _windUp = 0, float _overShoot = 0, ExecuteOnCompletion execute = null)
    {
        transform = t;
        start = t.position;
        target = _target;
        timerMax = totalTime;
        transitionType = type;
        baseTargetFromObject = fromObject;
        executeOnCompletion += execute;
        windUp = _windUp;
        overShoot = _overShoot;
        referenceStartingPosition = t.localPosition;
    }

    public MoveTransition(Transform t, Vector3 _target, float totalTime, TransitionType tStart, TransitionType tEnding, bool fromObject = false, ExecuteOnCompletion execute = null)
    {
        transform = t;
        start = t.position;
        target = _target;
        timerMax = totalTime;
        transitionStart = tStart;
        transitionEnding = tEnding;
        baseTargetFromObject = fromObject;
        executeOnCompletion += execute;
        referenceStartingPosition = t.localPosition;
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
                    t = TransitionSystem.NormalizedBezier3(windUp, overShoot, timer / timerMax);
                    break;
                default:
                    break;
            }
        }
        else if (transitionStart != null && transitionEnding != null)
        {
            float t2 = 0;

            switch (transitionStart)
            {
                case TransitionType.SmoothStart2:
                    t = TransitionSystem.SmoothStop2(timer / timerMax);
                    break;
                case TransitionType.SmoothStart3:
                    t = TransitionSystem.SmoothStop3(timer / timerMax);
                    break;
                case TransitionType.SmoothStart4:
                    t = TransitionSystem.SmoothStop4(timer / timerMax);
                    break;
                default:
                    break;
            }

            switch (transitionEnding)
            {
                case TransitionType.SmoothStop2:
                    t2 = TransitionSystem.SmoothStop2(timer / timerMax);
                    break;
                case TransitionType.SmoothStop3:
                    t2 = TransitionSystem.SmoothStop3(timer / timerMax);
                    break;
                case TransitionType.SmoothStop4:
                    t2 = TransitionSystem.SmoothStop4(timer / timerMax);
                    break;
                default:
                    break;
            }

            t = TransitionSystem.Crossfade(t, t2, timer / timerMax);
        }

        if (overShoot == 0 && windUp == 0)
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
            Vector2 temp = start + target * t;
            transform.position = temp;
        }

        base.Update();
    }

    public override void SafetyNet()
    {
        Vector3 finalPosition = referenceStartingPosition + (target / UIManager.Instance.ResolutionScaling);
        transform.localPosition = finalPosition;
        base.SafetyNet();
    }
}
