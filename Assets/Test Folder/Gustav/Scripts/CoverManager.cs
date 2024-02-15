using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Transition;

public class CoverManager : MonoBehaviour
{
    public static CoverManager Instance { get; private set; }

    private Transform topHalf;
    private Transform lowerHalf;

    private Vector3 originalPositionTop;
    private Vector3 originalPositionLower;

    private void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
            Destroy(this);
    }

    void Start()
    {
        topHalf = transform.GetChild(0);
        lowerHalf = transform.GetChild(1);

        originalPositionTop = topHalf.localPosition;
        originalPositionLower = lowerHalf.localPosition;

        UnCover(1, null);
    }

    public void UnCover(float time, ExecuteOnCompletion execute)
    {
        Vector3 topHalfDestination = originalPositionTop * UIManager.Instance.ResolutionScaling;
        Vector3 lowerHalfDestination = originalPositionLower * UIManager.Instance.ResolutionScaling;

        topHalfDestination.y *= 2;
        lowerHalfDestination.y *= 2;

        TransitionSystem.AddMoveTransition(new MoveTransition(topHalf, topHalfDestination, time, TransitionType.SmoothStop2, true, 0, 0, execute));
        TransitionSystem.AddMoveTransition(new MoveTransition(lowerHalf, lowerHalfDestination , time, TransitionType.SmoothStop2, true));
    }

    public void Cover(float time, ExecuteOnCompletion execute)
    {
        Vector3 topHalfDestination = -originalPositionTop * UIManager.Instance.ResolutionScaling;
        Vector3 lowerHalfDestination = -originalPositionLower * UIManager.Instance.ResolutionScaling;

        topHalfDestination.y *= 2;
        lowerHalfDestination.y *= 2;

        TransitionSystem.AddMoveTransition(new MoveTransition(topHalf, topHalfDestination, time, TransitionType.SmoothStop2, true, 0, 0, execute));
        TransitionSystem.AddMoveTransition(new MoveTransition(lowerHalf, lowerHalfDestination, time, TransitionType.SmoothStop2, true));
    }
}
