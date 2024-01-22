using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class UIInput : MonoBehaviour
{
    private UIManager manager;

    public void Start()
    {
        manager = GetComponent<UIManager>();
    }

    public void Navigate(InputAction.CallbackContext context)
    {
        if (UIManager.amountOfUIObjects.Count > 0)
        {
            if (context.performed)
            {
                if (manager.CurrentButten.GetComponent<SliderStateManager>() != null)
                {
                    if (manager.CurrentButten.GetComponent<UI>() != null)
                    {
                        if (manager.CurrentButten.GetComponent<UI>().activated)
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
        if (UIManager.amountOfUIObjects.Count > 0)
        {
            if (context.performed)
            {
                if (manager.CurrentButten.GetComponent<SliderStateManager>() != null && manager.CurrentButten.GetComponent<UI>() != null)
                {
                    SliderStateManager sliderSM = manager.CurrentButten.GetComponent<SliderStateManager>();
                    UI uI = manager.CurrentButten.GetComponent<UI>();

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
        if (UIManager.amountOfUIObjects.Count > 0)
        {
            if (manager.CurrentButten.GetComponent<UI>() != null)
            {
                UI uI = manager.CurrentButten.GetComponent<UI>();

                if (context.performed)
                {
                    if (manager.CurrentButten.GetComponent<SliderStateManager>() == null)
                    {
                        uI.activated = true;
                    }
                    else
                    {
                        bool temp = manager.CurrentButten.GetComponent<UI>().activated;

                        temp = !temp;

                        manager.CurrentButten.GetComponent<UI>().activated = temp;
                    }
                }
                if (context.canceled)
                {
                    if (manager.CurrentButten.GetComponent<SliderStateManager>() == null)
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
        if (UIManager.amountOfUIObjects.Count > 0 && !manager.KeyOrControlActive)
        {
            if (context.performed)
            {
                if (manager.CurrentButten.GetComponent<UI>() != null)
                {
                    manager.CurrentButten.GetComponent<UI>().activated = true;
                }
            }
            else if (context.canceled)
            {
                if (manager.CurrentButten.GetComponent<UI>() != null)
                {
                    manager.CurrentButten.GetComponent<UI>().activated = false;
                }
            }
        }
    }
}
