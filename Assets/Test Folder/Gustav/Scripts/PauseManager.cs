using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PauseManager : MonoBehaviour
{
    public Animator anim;

    public List<Image> images = new();
    public List<TextMeshProUGUI> text = new();

    private Color startingColor = new(1, 1, 1, 0);
    private Color newColor = new(1, 1, 1, 0.9f);

    public void Start()
    {
        foreach (BaseStateManager uI in GetComponentsInChildren<BaseStateManager>())
        {
            if (uI.Pointers != null)
            {
                uI.Pointers.SetActive(false);
            }
        }

        for (int i = 0; i < images.Count; i++)
        {
            images[i].color = startingColor;
        }
        for (int i = 0; i < text.Count; i++)
        {
            text[i].color = startingColor;
        }
    }

    public void InstantiateVisuals()
    {
        List<GameObject> list = new();

        foreach (UI uI in GetComponentsInChildren<UI>())
        {
            list.Add(uI.gameObject);
        }

        UIManager.Instance.LoadUI(list);
    }

    public void DestroyVisual()
    {

    }

    public void FadeIn()
    {
        UIInput.PauseTransitionFinished = true;

        for (int i = 0; i < images.Count; i++)
        {
            TransitionSystem.AddColorTransition(new ColorTransition(images[i], newColor, 0.2f, TransitionType.SmoothStop2));
        }
        for (int i = 0; i < text.Count; i++)
        {
            TransitionSystem.AddColorTransition(new ColorTransition(text[i], newColor, 0.2f, TransitionType.SmoothStop2));
        }

        List<GameObject> list = new();

        foreach (UI uI in GetComponentsInChildren<UI>())
        {
            if (!uI.IsDestroyed)
            {
                list.Add(uI.gameObject);
            }
        }

        UIManager.Instance.LoadUI(list);
    }
    public void FadeOut()
    {
        UIInput.PauseTransitionFinished = true;

        for (int i = 0; i < images.Count; i++)
        {
            TransitionSystem.AddColorTransition(new ColorTransition(images[i], startingColor, 0.2f, TransitionType.SmoothStop2));
        }
        for (int i = 0; i < text.Count; i++)
        {
            TransitionSystem.AddColorTransition(new ColorTransition(text[i], startingColor, 0.2f, TransitionType.SmoothStop2));
        }
    }

    public void MoveAway()
    {
        Transition.ExecuteOnCompletion execute = null;

        execute += ExecuteAfterMovingRemovingPauseMenu;

        UIInput.PauseGameObjectInstance.GetComponent<OnLoad>().MoveAway(UIInput.PauseGameObjectInstance, 1, execute);

        //Vector3 destination = new(Screen.width, 0);

        //Transform t = UIInput.PauseGameObjectInstance.transform;

        //TransitionSystem.AddMoveTransition(new MoveTransition(t, destination, 0.5f, TransitionType.SmoothStop2, transform, 0, 0, execute));
    }
    private void ExecuteAfterMovingRemovingPauseMenu()  
    {
        UIInput.PauseGameObjectInstance.transform.localPosition = Vector3.zero;
        UIInput.PauseGameObjectInstance.SetActive(false);
        UIInput.PauseMenuActive = false;
        UIManager.Instance.DisableTransitioning();
    }
}
