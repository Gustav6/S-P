using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PauseManager : MonoBehaviour
{
    public Animator anim;

    public GameObject visuals;

    public static bool Transitioning { get; private set; }

    public void EnableVisuals()
    {
        visuals.SetActive(true);
        LoadUIOnEnable(UIStateManager.Instance);
    }

    public void DisableVisuals()
    {
        visuals.SetActive(false);
    }

    public void EnablePauseMenu()
    {
        anim.SetBool("UnPaused", false);
    }

    public void DisablePauseMenu()
    {
        Transitioning = true;
        anim.SetBool("UnPaused", true);
    }

    public void CallOnDisable()
    {
        OnLoad onLoad = GetComponentInParent<OnLoad>();

        onLoad.CallOnDisable(ExecuteOnDisable);
    }

    private void ExecuteOnDisable()
    {
        transform.parent.localPosition = Vector3.zero;
        if (UIStateManager.Instance.PrefabToDisable != null)
        {
            UIStateManager.Instance.DisableUIPrefab();
        }
        Transitioning = false;
        UIStateManager.Instance.CursorInstance.SetActive(false);
    }

    public void LoadUIOnEnable(UIStateManager stateManager)
    {
        #region Reset variables
        stateManager.maxXPos = 0;
        stateManager.maxYPos = 0;
        stateManager.ListOfUIObjects.Clear();
        #endregion

        FindEveryInteractableUI(stateManager);
    }

    private void FindEveryInteractableUI(UIStateManager stateManager)
    {
        // Note that if there is no start position (0, 0) bugs will appear.
        #region Find objects with script UI and max X & Y value
        foreach (UI interactableUI in stateManager.GetComponentsInChildren<UI>())
        {
            if (interactableUI.isInteractable)
            {
                stateManager.CurrentUISelected = new Vector2(0, 0);

                if (interactableUI.position.y > stateManager.maxYPos)
                {
                    stateManager.maxYPos = interactableUI.position.y;
                }

                if (interactableUI.position.x > stateManager.maxXPos)
                {
                    stateManager.maxXPos = interactableUI.position.x;
                }

                stateManager.ListOfUIObjects.Add(interactableUI.gameObject);
            }
        }
        #endregion
    }
}
