using System.Collections;
using System.Threading;
using UnityEngine;
using static Transition;

public class UIManagerUnLoadedState : UIManagerBaseState
{
    public override void EnterState(UIStateManager stateManager)
    {
        stateManager.ActivePrefab = null;
        stateManager.CursorInstance.SetActive(false);
    }

    public override void UpdateState(UIStateManager stateManager)
    {
        if (stateManager.pausePrefab != null && stateManager.PauseMenuActive && !PauseManager.Transitioning)
        {
            stateManager.PlayTransition = false;
            stateManager.PrefabToEnable = stateManager.pausePrefab;
            stateManager.PauseInstance = stateManager.PrefabToEnable;
            stateManager.PauseInstance.transform.localPosition = Vector3.zero;
            stateManager.SwitchState(stateManager.ManagerTransitioningState);
        }
        else if (stateManager.PrefabToEnable != null)
        {
            stateManager.SwitchState(stateManager.ManagerTransitioningState);
        }
    }

    public override void ExitState(UIStateManager stateManager)
    {

    }
}

public class UIManagerTransitioningState : UIManagerBaseState
{
    public float transitionTime = 1;

    public override void EnterState(UIStateManager stateManager)
    {
        stateManager.EnableTransitioning();

        ExecuteOnCompletion execute = null;

        bool hasStartedCoroutine = false;

        if (stateManager.PrefabToEnable != null)
        {
            if (stateManager.PlayTransition && CoverManager.Instance != null)
            {
                execute += SwitchToLoadedState;
            }
            else
            {
                stateManager.StartCoroutine(WaitCoroutine());
                hasStartedCoroutine = true;
            }
        }
        else
        {
            if (stateManager.PlayTransition && CoverManager.Instance != null)
            {
                execute += SwitchToUnLoadedState;
            }
            else
            {
                SwitchToUnLoadedState();
            }
        }

        if (CoverManager.Instance != null && !hasStartedCoroutine)
        {
            if (stateManager.PlayTransition)
            {
                CoverManager.Instance.Cover(transitionTime, execute);
            }
        }
    }

    public override void UpdateState(UIStateManager stateManager)
    {

    }

    public override void ExitState(UIStateManager stateManager)
    {
        if (stateManager.PrefabToDisable != null)
        {
            stateManager.DisableUIPrefab();
        }
    }

    private IEnumerator WaitCoroutine()
    {
        yield return new WaitForSeconds(transitionTime / 2);

        SwitchToLoadedState();
    }

    public void SwitchToLoadedState()
    {
        UIStateManager.Instance.SwitchState(UIStateManager.Instance.ManagerLoadedState);
    }
    public void SwitchToUnLoadedState()
    {
        UIStateManager.Instance.SwitchState(UIStateManager.Instance.ManagerUnLoadedState);
    }
}

public class UIManagerLoadedState : UIManagerBaseState
{
    public override void EnterState(UIStateManager stateManager)
    {
        ExecuteOnCompletion execute = stateManager.DisableTransitioning;

        stateManager.EnableUIPrefab();

        stateManager.ActivePrefab = stateManager.PrefabToEnable;

        CallOnEnable(stateManager, execute);

        if (stateManager.PlayTransition && CoverManager.Instance != null)
        {
            CoverManager.Instance.UnCover(1, execute);
            stateManager.PlayTransition = false;
        }
        else
        {
            stateManager.DisableTransitioning();
        }

        stateManager.PrefabToDisable = null;
        stateManager.PrefabToEnable = null;

        LoadUI(stateManager);
    }

    public override void UpdateState(UIStateManager stateManager)
    {
        if (stateManager.PrefabToDisable != null)
        {
            if (!stateManager.Transitioning)
            {
                stateManager.SwitchState(stateManager.ManagerTransitioningState);
            }
        }

        if (stateManager.PauseInstance != null)
        {
            if (!stateManager.PauseMenuActive)
            {
                stateManager.PrefabToDisable = stateManager.PauseInstance;
                stateManager.SwitchState(stateManager.ManagerUnLoadedState);
            }
        }

        if (stateManager.KeyOrControlActive)
        {
            if (stateManager.CursorInstance != null)
            {
                stateManager.CursorInstance.SetActive(false);
            }
        }
        else
        {
            if (stateManager.CursorInstance != null)
            {
                stateManager.CursorInstance.SetActive(true);
                Debug.Log("Cursor on");
            }
        }
    }

    public override void ExitState(UIStateManager stateManager)
    {
        ExecuteOnCompletion execute = null;

        if (stateManager.PauseInstance != null)
        {
            PauseManager pauseManager = stateManager.PauseInstance.GetComponentInChildren<PauseManager>();

            pauseManager.UnPauseGame();
        }
        else
        {
            CallOnDisable(stateManager, execute);
        }
    }

    public void CallOnDisable(UIStateManager stateManager, ExecuteOnCompletion execute)
    {
        if (stateManager.PrefabToDisable.GetComponent<OnLoad>() != null)
        {
            stateManager.PrefabToDisable.GetComponent<OnLoad>().CallOnDisable(execute);
        }
        else if (stateManager.GetComponentInChildren<OnLoad>() != null)
        {
            foreach (OnLoad gameObject in stateManager.GetComponentsInChildren<OnLoad>())
            {
                gameObject.CallOnDisable(execute);
            }
        }
    }

    public void CallOnEnable(UIStateManager stateManager, ExecuteOnCompletion execute)
    {
        if (stateManager.PrefabToEnable.GetComponent<OnLoad>() != null)
        {
            stateManager.PrefabToEnable.GetComponent<OnLoad>().CallOnEnable(execute);
        }
        else if (stateManager.GetComponentInChildren<OnLoad>() != null)
        {
            foreach (OnLoad gameObject in stateManager.GetComponentsInChildren<OnLoad>())
            {
                gameObject.CallOnEnable(execute);
            }
        }
    }

    public void LoadUI(UIStateManager stateManager)
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
