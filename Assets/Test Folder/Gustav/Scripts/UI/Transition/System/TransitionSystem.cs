using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class TransitionSystem
{
    public static List<Transition> transitions = new();

    public static void Update()
    {
        for (int i = 0; i < transitions.Count; i++)
        {
            transitions[i].Update();
        }

        for (int i = transitions.Count - 1; i >= 0; i--)
        {
            if (transitions[i].isRemoved)
            {
                transitions[i].actionDelegate?.Invoke();
                transitions.RemoveAt(i);
            }
        }
    }

    public static void AddMoveTransition(MoveTransition moveTransition)
    {
        transitions.Add(moveTransition);
    }
    public static void AddScaleTransition(ScaleTransition scaleTransition)
    {
        transitions.Add(scaleTransition);
    }
    public static void AddColorTransition(ColorTransition colorTransition)
    {
        transitions.Add(colorTransition);
    }

    public static float Flip(float t)
    {
        return 1 - t;
    }

    public static float SmoothStart2(float t)
    {
        return t * t;
    }

    public static float SmoothStart3(float t)
    {
        return t * t * t;
    }

    public static float SmoothStart4(float t)
    {
        return t * t * t * t;
    }

    public static float SmoothStop2(float t)
    {
        return 1 - (1 - t) * (1 - t);
    }

    public static float SmoothStop3(float t)
    {
        return 1 - (1 - t) * (1 - t) * (1 - t);
    }

    public static float SmoothStop4(float t)
    {
        return 1 - (1 - t) * (1 - t) * (1 - t) * (1 - t);
    }

    public static float Crossfade(float a, float b, float t)
    {
        return (1 - t) * a + t * b;
    }
}

public enum TransitionType
{
    SmoothStart2,
    SmoothStart3,
    SmoothStart4,

    SmoothStop2,
    SmoothStop3,
    SmoothStop4,

    bounceClampTop, 
    bounceClampBottom, 
    bounceClampBottomTop,
}

public enum TransitionStart
{
    SmoothStart2,
    SmoothStart3,
    SmoothStart4,
}
public enum TransitionEnd
{
    SmoothStop2,
    SmoothStop3,
    SmoothStop4,
}