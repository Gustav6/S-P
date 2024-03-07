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
    public void Pause(InputAction.CallbackContext context)
    {
        if (context.performed && !UIStateManager.Instance.Transitioning)
        {
            if (UIStateManager.Instance.ActivePrefab != null && UIStateManager.Instance.pausePrefab != null)
            {
                if (UIStateManager.Instance.ActivePrefab == UIStateManager.Instance.pausePrefab)
                {
                    UIStateManager.Instance.PauseMenuActive = !UIStateManager.Instance.PauseMenuActive;
                }
            }
            else if (UIStateManager.Instance.ActivePrefab == null)
            {
                UIStateManager.Instance.PauseMenuActive = !UIStateManager.Instance.PauseMenuActive;
            }
        }
    }

    public void Navigate(InputAction.CallbackContext context)
    {
        if (UIIsLoaded())
        {
            if (context.performed)
            {
                GameObject g = UIStateManager.Instance.FindInteractableUI(UIStateManager.Instance.CurrentUISelected);

                if (g.GetComponent<SliderStateManager>() != null || g.GetComponent<InputFieldStateManager>() != null)
                {
                    if (g.GetComponent<BaseStateManager>().UIActivated)
                    {
                        return;
                    }
                }

                if (!UIStateManager.Instance.KeyOrControlActive)
                {
                    UIStateManager.Instance.KeyOrControlActive = true;
                    return;
                }

                #region Change selected UI object
                Vector2 temp = UIStateManager.Instance.CurrentUISelected;

                if (context.ReadValue<Vector2>().y > 0)
                {
                    temp.y--;
                    UIStateManager.Instance.CurrentUISelected = temp;
                }
                else if (context.ReadValue<Vector2>().y < 0)
                {
                    temp.y++;
                    UIStateManager.Instance.CurrentUISelected = temp;
                }

                if (context.ReadValue<Vector2>().x < 0)
                {
                    temp.x--;
                    UIStateManager.Instance.CurrentUISelected = temp;
                }
                else if (context.ReadValue<Vector2>().x > 0)
                {
                    temp.x++;
                    UIStateManager.Instance.CurrentUISelected = temp;
                }
                #endregion
            }
        }
    }

    public void ChangeSlider(InputAction.CallbackContext context)
    {
        if (UIIsLoaded() && CanInteractWithSelectedUI())
        {
            SliderStateManager sm;
            GameObject g;

            g = UIStateManager.Instance.FindInteractableUI(UIStateManager.Instance.CurrentUISelected);

            if (g.GetComponent<SliderStateManager>() != null)
            {
                sm = g.GetComponent<SliderStateManager>();

                if (UIStateManager.Instance.ChangingSlider)
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

    public void Submit(InputAction.CallbackContext context)
    {
        if (UIIsLoaded() && CanInteractWithSelectedUI())
        {
            GameObject g = UIStateManager.Instance.FindInteractableUI(UIStateManager.Instance.CurrentUISelected);
            BaseStateManager referenceManager = g.GetComponent<BaseStateManager>();

            if (context.performed)
            {
                if (g.GetComponent<Slider>() == null && g.GetComponent<InputField>() == null)
                {
                    referenceManager.UIActivated = true;
                }
                else
                {
                    bool temp = referenceManager.UIActivated;

                    temp = !temp;

                    referenceManager.UIActivated = temp;
                }
            }
            if (context.canceled)
            {
                if (g.GetComponent<SliderStateManager>() == null && g.GetComponent<InputFieldStateManager>() == null)
                {
                    referenceManager.UIActivated = false;
                }
            }
        }
    }

    public void Pointer(InputAction.CallbackContext context)
    {
        if (UIStateManager.Instance.KeyOrControlActive)
        {
            if (context.ReadValue<Vector2>() != Vector2.zero)
            {
                UIStateManager.Instance.KeyOrControlActive = false;
            }
        }
    }

    public void ClickOnGameObject(InputAction.CallbackContext context)
    {
        if (UIIsLoaded() && CanInteractWithSelectedUI() && !UIStateManager.Instance.KeyOrControlActive)
        {
            BaseStateManager stateManager = UIStateManager.Instance.FindInteractableUI(UIStateManager.Instance.CurrentUISelected).GetComponent<BaseStateManager>();

            if (stateManager.UIInstance.hovering)
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
}
