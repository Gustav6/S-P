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
    public static bool PauseTransitionFinished { get; set; }
    public static bool PauseMenuActive { get; set; }

    public static GameObject PauseGameObjectInstance { get; set; }

    private void Start()
    {
        PauseTransitionFinished = true;

        if (UIManager.Instance.GetComponentInChildren<PauseManager>() != null)
        {
            PauseGameObjectInstance = UIManager.Instance.GetComponentInChildren<PauseManager>().transform.parent.gameObject;
            PauseGameObjectInstance.SetActive(false);
        }
    }

    public void Pause(InputAction.CallbackContext context)
    {
        if (context.performed && !UIManager.Instance.Transitioning)
        {
            UIManager.Instance.Paused = !UIManager.Instance.Paused;

            if (UIManager.Instance.Paused && !PauseMenuActive)
            {
                PauseTransitionFinished = false;
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
            else if (!UIManager.Instance.Paused && PauseMenuActive)
            {
                //manager.pausePrefab.GetComponent<ActiveMenuManager>().DisableBlur(UIManager.instance.CameraInstance.GetComponent<Blur>());
                OnUnPause();
            }
        }
    }

    private void OnPause()
    {
        PauseGameObjectInstance.SetActive(true);

        PauseMenuActive = true;
    }

    private void OnUnPause()
    {
        if (GetComponentInChildren<PauseManager>() != null)
        {
            GetComponentInChildren<PauseManager>().anim.SetTrigger("UnPaused");
        }
    }

    public void Navigate(InputAction.CallbackContext context)
    {
        if (CanInteractWithUI())
        {
            if (context.performed)
            {
                GameObject g = UIManager.Instance.CheckForInteractableUI(UIManager.Instance.CurrentUISelected);

                if (g.GetComponent<SliderStateManager>() != null || g.GetComponent<InputFieldStateManager>() != null)
                {
                    if (g.GetComponent<BaseStateManager>().UIActivated)
                    {
                        return;
                    }
                }

                if (!UIManager.Instance.KeyOrControlActive)
                {
                    UIManager.Instance.KeyOrControlActive = true;
                    return;
                }

                #region Change selected UI object
                Vector2 temp = UIManager.Instance.CurrentUISelected;

                if (context.ReadValue<Vector2>().y > 0)
                {
                    temp.y--;
                    UIManager.Instance.CurrentUISelected = temp;
                }

                if (context.ReadValue<Vector2>().y < 0)
                {
                    temp.y++;
                    UIManager.Instance.CurrentUISelected = temp;
                }

                if (context.ReadValue<Vector2>().x < 0)
                {
                    temp.x--;
                    UIManager.Instance.CurrentUISelected = temp;
                }

                if (context.ReadValue<Vector2>().x > 0)
                {
                    temp.x++;
                    UIManager.Instance.CurrentUISelected = temp;
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

            if (UIManager.Instance.CheckForInteractableUI(UIManager.Instance.CurrentUISelected) != null)
            {
                g = UIManager.Instance.CheckForInteractableUI(UIManager.Instance.CurrentUISelected);

                if (g.GetComponent<SliderStateManager>() != null)
                {
                    sm = g.GetComponent<SliderStateManager>();

                    if (UIManager.Instance.ChangingSlider)
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
            GameObject g = UIManager.Instance.CheckForInteractableUI(UIManager.Instance.CurrentUISelected);
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
        UIManager.Instance.MousePosition = context.ReadValue<Vector2>();
    }

    public void ClickOnGameObject(InputAction.CallbackContext context)
    {
        if (CanInteractWithUI() && !UIManager.Instance.KeyOrControlActive)
        {
            BaseStateManager stateManager = UIManager.Instance.CheckForInteractableUI(UIManager.Instance.CurrentUISelected).GetComponent<BaseStateManager>();

            if (UIManager.Instance.Hovering(stateManager.UIInstance.gameObject))
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
        if (UIManager.Instance.ListOfUIObjects.Count > 0 && !UIManager.Instance.Transitioning)
        {
            return true;
        }

        return false;
    }
}
