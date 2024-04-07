using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Credits : MonoBehaviour
{
    public GameObject creditsPrefab;
    public GameObject currentPrefab;

    [SerializeField] private List<Functions> selectedFunctions = new();
    private Dictionary<Functions, Action> functionLookup;

    private Selectable script;

    private void Start()
    {
        script = gameObject.GetComponent<Selectable>();
        functionLookup = new Dictionary<Functions, System.Action>()
        {
            { Functions.ShowCredits, ShowCredits }
        };
    }

    private void ShowCredits()
    {
        UIStateManager.Instance.PrefabToEnable = creditsPrefab;
        UIStateManager.Instance.PrefabToDisable = currentPrefab;
        UIStateManager.Instance.PlayTransition = true;
    }

    public void ActivateSelectedFunctions()
    {
        for (int i = 0; i < selectedFunctions.Count; i++)
        {
            functionLookup[selectedFunctions[i]]?.Invoke();
        }
    }


    public bool UIIsLoaded()
    {
        if (UIStateManager.Instance != null)
        {
            if (UIStateManager.Instance.ListOfUIObjects.Count > 0)
            {
                if (!UIStateManager.Instance.Transitioning)
                {
                    return true;
                }
            }
        }

        return false;
    }

    public bool CanInteractWithSelectedUI()
    {
        if (UIStateManager.Instance != null)
        {
            UIStateManager uIManager = UIStateManager.Instance;

            if (uIManager.FindInteractableUI(uIManager.CurrentUISelected).GetComponent<UI>() != null)
            {
                if (uIManager.FindInteractableUI(uIManager.CurrentUISelected).GetComponent<UI>().isInteractable)
                {
                    return true;
                }
            }
        }

        return false;
    }

    private enum Functions
    {
        ShowCredits
    }
}
