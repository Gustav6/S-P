using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using static Transition;

public class OnLoad : MonoBehaviour
{
    [Header("Drag image here")]
    public List<Image> images = new();

    [Header("Fade in in variables")]
    public bool fadeIn;
    public Color color;
    public float fadeInTime = 1.5f;

    [Header("Move in variables")]
    public bool moveIn;
    public float moveInTime = 1;
    public float moveOutTime = 1;
    public PrefabMoveDirection moveTowardsOnEnable;
    public PrefabMoveDirection moveTowardsOnDisable;

    public TransitionType transitionType = TransitionType.SmoothStop2;

    [Header("Scale in variables")]
    public bool scaleIn;
    public float scaleInTime = 1;
    public float scaleInResetTime = 1;
    public Vector3 startingScale;


    public void CallOnEnable(ExecuteOnCompletion execute)
    {
        if (fadeIn)
        {
            for (int i = 0; i < images.Count; i++)
            {
                Fade(images[i], fadeInTime, color);
            }
        }

        if (moveIn)
        {
            MoveIn(moveInTime, execute);
        }

        if (scaleIn)
        {
            Scale(execute);
        }
    }

    public void CallOnDisable(ExecuteOnCompletion execute)
    {
        if (moveTowardsOnDisable != PrefabMoveDirection.None)
        {
            MoveOut(moveOutTime, execute);
        }
    }

    private void MoveIn(float time, ExecuteOnCompletion execute)
    {
        Vector3 startingPosition = GiveDestination(moveTowardsOnEnable) * -1;
        startingPosition.x += Screen.width / 2;
        startingPosition.y += Screen.height / 2;
        transform.position = startingPosition;

        MoveUI(time, moveTowardsOnEnable, execute);
    }

    private void MoveOut(float time, ExecuteOnCompletion execute)
    {
        MoveUI(time, moveTowardsOnDisable, execute);
    }

    private void Scale(ExecuteOnCompletion execute)
    {
        execute += ResetScale;

        TransitionSystem.AddScaleTransition(new ScaleTransition(transform, startingScale, scaleInTime, TransitionType.SmoothStop2, execute));
    }

    public void ResetScale()
    {
        TransitionSystem.AddScaleTransition(new ScaleTransition(transform, Vector3.one, scaleInResetTime, TransitionType.SmoothStop2));
    }

    public void Fade(Image image, float fadeInTIme, Color newColor)
    {
        image.color = new(0, 0, 0, 0);
        TransitionSystem.AddColorTransition(new ColorTransition(image, newColor, fadeInTIme, TransitionType.SmoothStart2));
    }

    private void MoveGameObject(GameObject g, float time, Vector3 destination, ExecuteOnCompletion execute = null)
    {
        TransitionSystem.AddMoveTransition(new MoveTransition(g.transform, destination, time, transitionType, true, 0, 0, execute));
    }

    public void MoveUI(float time, PrefabMoveDirection direction, ExecuteOnCompletion execute)
    {
        Vector3 destination = GiveDestination(direction);

        MoveGameObject(gameObject, time, destination, execute);
    }

    public Vector3 GiveDestination(PrefabMoveDirection direction)
    {
        Vector3 temp = Vector3.zero;

        switch (direction)
        {
            case PrefabMoveDirection.Left:
                temp = new(-Screen.width, 0, 0);
                break;
            case PrefabMoveDirection.Right:
                temp = new(Screen.width, 0, 0);
                break;
            case PrefabMoveDirection.Up:
                temp = new(0, Screen.height, 0);
                break;
            case PrefabMoveDirection.Down:
                temp = new(0, -Screen.height);
                break;
            default:
                break;
        }

        return temp;
    }
}
