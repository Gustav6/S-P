using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class OnLoad : MonoBehaviour
{
    [Header("Drag Image Here")]
    public Image image;
    public Color color;
    public float fadeInTime = 1.5f;
    public bool fadeIn;

    [Header("Drag Image Here")]
    public bool moveIn;
    public float moveInTime = 1;

    public void Start()
    {
        if (fadeIn)
        {
            FadeIn(image, fadeInTime, color);
        }
        else if (moveIn)
        {
            Transition.ExecuteOnCompletion execute = null;
            execute += UIManager.instance.DisableTransitioning;

            Vector3 startingPosition = UIManager.instance.GiveDestination(GetComponent<ActiveBaseManager>().moveTowardsOnStart);

            transform.localPosition = startingPosition;
            UIManager.instance.MoveUIToStart(moveInTime, gameObject, execute);
        }
    }

    private void FadeIn(Image image, float fadeInTIme, Color newColor)
    {
        TransitionSystem.AddColorTransition(new ColorTransition(image, newColor, fadeInTIme, TransitionType.SmoothStart2));
    }
}
