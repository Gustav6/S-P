using NUnit.Framework;
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class UIInput : MonoBehaviour
{
    private UIManager manager;
    private bool hasRemovedPauseMenu = true;

    public void Start()
    {
        manager = GetComponent<UIManager>();
    }

    public void Pause(InputAction.CallbackContext context)
    {
        if (context.performed && !UIManager.instance.Transitioning)
        {
            UIManager.instance.Paused = !UIManager.instance.Paused;

            if (UIManager.instance.Paused && hasRemovedPauseMenu)
            {
                OnPause();

                //Scene scene = SceneManager.GetActiveScene();

                //if (scene.buildIndex == (int)NewScene.Game)
                //{
                //    if (manager.pausePrefab != null)
                //    {
                //        UIManager.instance.InstantiateNewUIPrefab(manager.pausePrefab, GetComponent<UIManager>().transform, Vector3.one, Vector3.zero);
                //    }
                //}
            }
            else if (!UIManager.instance.Paused && !hasRemovedPauseMenu)
            {
                //manager.pausePrefab.GetComponent<ActiveMenuManager>().DisableBlur(UIManager.instance.CameraInstance.GetComponent<Blur>());
                OnUnPause();
            }
        }
    }

    private void OnPause()
    {
        Transition.ExecuteOnCompletion execute = null;

        Vector3 spawnLocation = UIManager.instance.GiveDestination(PrefabMoveDirection.Right);

        hasRemovedPauseMenu = false;

        execute += ExecuteAfterMovingAddedPauseMenu;

        UIManager.instance.InstantiateNewUIPrefab(manager.pausePrefab, GetComponent<UIManager>().transform, Vector3.one, -spawnLocation);

        Vector3 destionation = new(Screen.width / 2, 0);
        destionation.y += UIManager.instance.CurrentUIPrefab.transform.position.y;

        TransitionSystem.AddMoveTransition(new MoveTransition(UIManager.instance.CurrentUIPrefab.transform, destionation, 0.75f, TransitionType.SmoothStop2, false, 0, 0, execute));
    }

    private void OnUnPause()
    {
        Transition.ExecuteOnCompletion execute = null;

        execute += ExecuteAfterMovingRemovingPauseMenu;

        Vector3 destionation = new(-Screen.width / 2, 0);
        destionation.y += UIManager.instance.CurrentUIPrefab.transform.position.y;

        TransitionSystem.AddMoveTransition(new MoveTransition(UIManager.instance.CurrentUIPrefab.transform, destionation, 0.5f, TransitionType.SmoothStop2, false, 0, 0, execute));
    }

    private void ExecuteAfterMovingAddedPauseMenu()
    {
        List<GameObject> list = new();

        foreach (UI uI in GetComponentsInChildren<UI>())
        {
            if (!uI.IsDestroyed)
            {
                list.Add(uI.gameObject);
            }
        }

        UIManager.instance.LoadUI(list);
    }
    private void ExecuteAfterMovingRemovingPauseMenu()
    {
        Destroy(UIManager.instance.CurrentUIPrefab);
        UIManager.instance.ResetListOfUIObjects();
        hasRemovedPauseMenu = true;
    }

    public void Navigate(InputAction.CallbackContext context)
    {
        if (CanInteractWithUI())
        {
            if (context.performed)
            {
                GameObject g = manager.CheckForInteractableUI(manager.CurrentUISelected);

                if (g.GetComponent<SliderStateManager>() != null || g.GetComponent<InputFieldStateManager>() != null)
                {
                    if (g.GetComponent<BaseStateManager>().UIActivated)
                    {
                        return;
                    }
                }

                if (!manager.KeyOrControlActive)
                {
                    manager.KeyOrControlActive = true;
                    return;
                }

                #region Change selected UI object
                Vector2 temp = manager.CurrentUISelected;

                if (context.ReadValue<Vector2>().y > 0)
                {
                    temp.y--;
                    manager.CurrentUISelected = temp;
                }

                if (context.ReadValue<Vector2>().y < 0)
                {
                    temp.y++;
                    manager.CurrentUISelected = temp;
                }

                if (context.ReadValue<Vector2>().x < 0)
                {
                    temp.x--;
                    manager.CurrentUISelected = temp;
                }

                if (context.ReadValue<Vector2>().x > 0)
                {
                    temp.x++;
                    manager.CurrentUISelected = temp;
                }
                #endregion
            }
        }
    }

    public void ChangeSlider(InputAction.CallbackContext context)
    {
        if (CanInteractWithUI())
        {
            SliderStateManager sm;
            GameObject g;

            if (manager.CheckForInteractableUI(manager.CurrentUISelected) != null)
            {
                g = manager.CheckForInteractableUI(manager.CurrentUISelected);

                if (g.GetComponent<SliderStateManager>() != null)
                {
                    sm = g.GetComponent<SliderStateManager>();

                    if (manager.ChangingSlider)
                    {
                        if (context.ReadValue<float>() < 0)
                        {
                            sm.moveDirection = -1;
                        }
                        else if (context.ReadValue<float>() > 0)
                        {
                            sm.moveDirection = 1;
                        }
                    }

                    if (context.canceled)
                    {
                        sm.moveDirection = 0;
                    }
                }
            }
        }
    }

    public void Submit(InputAction.CallbackContext context)
    {
        if (CanInteractWithUI())
        {
            GameObject g = manager.CheckForInteractableUI(manager.CurrentUISelected);
            BaseStateManager uI = g.GetComponent<BaseStateManager>();

            if (context.performed)
            {
                if (g.GetComponent<Slider>() == null && g.GetComponent<InputField>() == null)
                {
                    uI.UIActivated = true;
                }
                else
                {
                    bool temp = uI.UIActivated;

                    temp = !temp;

                    uI.UIActivated = temp;
                }
            }
            if (context.canceled)
            {
                if (g.GetComponent<SliderStateManager>() == null && g.GetComponent<InputFieldStateManager>() == null)
                {
                    uI.UIActivated = false;
                }
            }
        }
    }

    public void PointerPosition(InputAction.CallbackContext context)
    {
        manager.MousePosition = context.ReadValue<Vector2>();
    }

    public void ClickOnGameObject(InputAction.CallbackContext context)
    {
        if (CanInteractWithUI() && !manager.KeyOrControlActive)
        {
            BaseStateManager stateManager = manager.CheckForInteractableUI(manager.CurrentUISelected).GetComponent<BaseStateManager>();

            if (UIManager.instance.Hovering(stateManager.UIInstance.gameObject))
            {
                if (context.performed)
                {
                    stateManager.UIActivated = true;
                }
            }

            if (context.canceled)
            {
                stateManager.UIActivated = false;
            }
        }
    }

    public bool CanInteractWithUI()
    {
        if (UIManager.instance.ListOfUIObjects.Count > 0 && !UIManager.instance.Transitioning)
        {
            return true;
        }

        return false;
    }
}
