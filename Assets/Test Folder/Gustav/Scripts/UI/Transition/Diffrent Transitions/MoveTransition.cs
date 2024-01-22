using UnityEngine;
using UnityEngine.UI;

public class MoveTransition : Transition
{
    public Transform transform;
    public Vector3 start;
    public Vector3 target;
    private readonly TransitionType? transitionType;
    private readonly TransitionStart transitionStart;
    private readonly TransitionEnd transitionEnding;
    private readonly bool baseTargetFromObject;

    public MoveTransition(Transform _transform, Vector3 _target, float timeItTakes, TransitionType _transitionType, bool _baseTargetFromObject = false)
    {
        transform = _transform;
        start = _transform.position;
        target = _target;
        timerMax = timeItTakes;
        transitionType = _transitionType;
        baseTargetFromObject = _baseTargetFromObject;
    }

    public MoveTransition(Transform _transform, Vector3 _target, float timeItTakes, TransitionStart _transitionStart, TransitionEnd _transitionEnding, bool _baseTargetFromObject = false)
    {
        transform = _transform;
        start = transform.position;
        target = _target;
        timerMax = timeItTakes;
        transitionStart = _transitionStart;
        transitionEnding = _transitionEnding;
        baseTargetFromObject = _baseTargetFromObject;
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
                case TransitionType.bounceClampTop:
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

        if (!baseTargetFromObject)
        {
            transform.position = Vector3.Lerp(start, target, t);
        }
        else
        {
            transform.position = Vector3.Lerp(start, start + target, t);
        }

        base.Update();
    }
}
