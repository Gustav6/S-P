using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestCurve : MonoBehaviour
{
    void Start()
    {

    }

    private void Update()
    {
        if (Input.GetKeyUp(KeyCode.Return))
        {
            TransitionSystem.AddMoveTransition(new MoveTransition(transform, new Vector3(2, 2, 0), 1, TransitionBezier.NormalizedBezier3, false, null, -2, 2));
        }
    }
}
