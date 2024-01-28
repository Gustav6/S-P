using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class UIInput : MonoBehaviour
{
    private UIManager manager;
    private bool paused = false;
    private GameObject temp;

    public void Start()
    {
        manager = GetComponent<UIManager>();
    }

    public void Pause(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            paused = !paused;

            if (paused)
            {
                Scene scene = SceneManager.GetActiveScene();

                if (scene.buildIndex == (int)NewScene.Game)
                {
                    if (manager.PausePrefab != null)
                    {
                        UIManager.InstantiateNewUIPrefab(manager.PausePrefab, GetComponent<UIManager>().transform);
                    }
                }
            }
            else
            {
                Destroy(UIManager.CurrentUIGameObject);
            }
        }
    }

    public void Navigate(InputAction.CallbackContext context)
    {
        if (UIManager.AmountOfUIObjects.Count > 0)
        {
            if (context.performed)
            {
                if (manager.CurrentUiElement.GetComponent<SliderStateManager>() != null)
                {
                    if (manager.CurrentUiElement.GetComponent<UI>() != null)
                    {
                        if (manager.CurrentUiElement.GetComponent<UI>().activated)
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
        if (UIManager.AmountOfUIObjects.Count > 0)
        {
            if (context.performed)
            {
                if (manager.CurrentUiElement.GetComponent<SliderStateManager>() != null && manager.CurrentUiElement.GetComponent<UI>() != null)
                {
                    SliderStateManager sliderSM = manager.CurrentUiElement.GetComponent<SliderStateManager>();
                    UI uI = manager.CurrentUiElement.GetComponent<UI>();

                    if (uI.activated)
                    {
                        if (context.ReadValue<float>() < 0)
                        {
                            sliderSM.moveDirection = -1;
                        }
                        else if (context.ReadValue<float>() > 0)
                        {
                            sliderSM.moveDirection = 1;
                        }
                        else
                        {
                            sliderSM.moveDirection = 0;
                        }
                    }
                }
            }
        }
    }

    public void Submit(InputAction.CallbackContext context)
    {
        if (UIManager.AmountOfUIObjects.Count > 0)
        {
            if (manager.CurrentUiElement.GetComponent<UI>() != null)
            {
                UI uI = manager.CurrentUiElement.GetComponent<UI>();

                if (context.performed)
                {
                    if (manager.CurrentUiElement.GetComponent<SliderStateManager>() == null)
                    {
                        uI.activated = true;
                    }
                    else
                    {
                        bool temp = manager.CurrentUiElement.GetComponent<UI>().activated;

                        temp = !temp;

                        manager.CurrentUiElement.GetComponent<UI>().activated = temp;
                    }
                }
                if (context.canceled)
                {
                    if (manager.CurrentUiElement.GetComponent<SliderStateManager>() == null)
                    {
                        uI.activated = false;
                    }
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
        if (UIManager.AmountOfUIObjects.Count > 0 && !manager.KeyOrControlActive)
        {
            if (manager.HoveringGameObject(manager.CurrentUiElement))
            {
                if (context.performed)
                {
                    if (manager.CurrentUiElement.GetComponent<UI>() != null)
                    {
                        manager.CurrentUiElement.GetComponent<UI>().activated = true;
                    }
                }
                else if (context.canceled)
                {
                    if (manager.CurrentUiElement.GetComponent<UI>() != null)
                    {
                        manager.CurrentUiElement.GetComponent<UI>().activated = false;
                    }
                }
            }
        }
    }
}
