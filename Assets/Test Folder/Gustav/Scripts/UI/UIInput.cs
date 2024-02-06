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

    public void Start()
    {
        manager = GetComponent<UIManager>();
    }

    public void Pause(InputAction.CallbackContext context)
    {
        if (context.performed && !UIManager.Transitioning)
        {
            UIManager.Paused = !UIManager.Paused;

            if (UIManager.Paused)
            {
                Scene scene = SceneManager.GetActiveScene();

                if (scene.buildIndex == (int)NewScene.Game)
                {
                    if (manager.pausePrefab != null)
                    {
                        UIManager.InstantiateNewUIPrefab(manager.pausePrefab, GetComponent<UIManager>().transform, Vector3.one, Vector3.zero);
                    }
                }
            }
            else
            {
                manager.pausePrefab.GetComponent<ActiveMenuManager>().DisableBlur(UIManager.CameraInstance.GetComponent<Blur>());
                Destroy(UIManager.CurrentUIPrefab);
                UIManager.ResetListOfUIObjects();
            }
        }
    }

    public void Navigate(InputAction.CallbackContext context)
    {
        if (CanInteractWithUI())
        {
            if (context.performed)
            {
                GameObject g = manager.CheckForInteractableUI(manager.currentUISelected).gameObject;

                if (g.GetComponent<SliderStateManager>() != null)
                {
                    if (g.GetComponent<UI>() != null)
                    {
                        if (g.GetComponent<UI>().activated)
                        {
                            return;
                        }
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

            if (manager.CheckForInteractableUI(manager.currentUISelected).gameObject != null)
            {
                g = manager.CheckForInteractableUI(manager.currentUISelected).gameObject;

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
            GameObject g = manager.CheckForInteractableUI(manager.currentUISelected).gameObject;
            UI uI = g.GetComponent<UI>();

            if (context.performed)
            {
                if (g.GetComponent<SliderStateManager>() == null)
                {
                    uI.activated = true;
                }
                else
                {
                    bool temp = uI.activated;

                    temp = !temp;

                    uI.activated = temp;
                }
            }
            if (context.canceled)
            {
                if (g.GetComponent<SliderStateManager>() == null)
                {
                    uI.activated = false;
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
            GameObject g = manager.CheckForInteractableUI(manager.currentUISelected).gameObject;
            UI uI = g.GetComponent<UI>();

            if (manager.HoveringGameObject(g))
            {
                if (context.performed)
                {
                    uI.activated = true;
                }
            }

            if (context.canceled)
            {
                uI.activated = false;
            }
        }
    }

    public bool CanInteractWithUI()
    {
        if (UIManager.ListOfUIObjects.Count > 0 && !UIManager.Transitioning)
        {
            return true;
        }

        return false;
    }
}
