using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestCurve : MonoBehaviour
{
    void Start()
    {
        TransitionSystem.AddScaleTransition(new ScaleTransition(transform, new Vector3(2, 2, 1), 1, TransitionType.NormalizedBezier3, null, 0, 1));
    }
}
