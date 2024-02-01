using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class UIInput : MonoBehaviour
{
    private UIManager manager;
    private bool paused = false;

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
                        UIManager.InstantiateNewUIPrefab(manager.PausePrefab, GetComponent<UIManager>().transform, Vector3.one, Vector3.zero);
                    }
                }
            }
            else
            {
                Destroy(UIManager.CurrentUIPrefab);
                UIManager.ResetListOfUIObjects();
            }
        }
    }

    public void Navigate(InputAction.CallbackContext context)
    {
        if (UIManager.ListOfUIObjects.Count > 0)
        {
            if (context.performed)
            {
                try
                {
                    GameObject g = manager.FindUIElement(manager.currentUISelected).gameObject;

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
                }
                catch (NullReferenceException)
                {
                    Debug.Log("Navigate problem");
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
        if (UIManager.ListOfUIObjects.Count > 0 && manager.ChangingSlider)
        {
            if (context.performed)
            {
                try
                {
                    GameObject g = manager.FindUIElement(manager.currentUISelected).gameObject;
                    SliderStateManager sm = g.GetComponent<SliderStateManager>();

                    if (context.ReadValue<float>() < 0)
                    {
                        sm.moveDirection = -1;
                    }
                    else if (context.ReadValue<float>() > 0)
                    {
                        sm.moveDirection = 1;
                    }
                    else
                    {
                        sm.moveDirection = 0;
                    }
                }
                catch (NullReferenceException)
                {
                    Debug.Log("Cant change slider");
                }
            }
        }
    }

    public void Submit(InputAction.CallbackContext context)
    {
        if (UIManager.ListOfUIObjects.Count > 0)
        {
            try
            {
                GameObject g = manager.FindUIElement(manager.currentUISelected).gameObject;
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
            catch (NullReferenceException)
            {
                Debug.Log("Cant submit");
            }
        }
    }

    public void PointerPosition(InputAction.CallbackContext context)
    {
        manager.MousePosition = context.ReadValue<Vector2>();
    }

    public void ClickOnGameObject(InputAction.CallbackContext context)
    {
        if (UIManager.ListOfUIObjects.Count > 0 && !manager.KeyOrControlActive)
        {
            if (context.performed)
            {
                try
                {
                    GameObject g = manager.FindUIElement(manager.currentUISelected).gameObject;
                    UI uI = g.GetComponent<UI>();

                    if (manager.HoveringGameObject(g))
                    {
                        if (context.performed)
                        {
                            uI.activated = true;
                        }
                        else if (context.canceled)
                        {
                            uI.activated = false;
                        }
                    }
                }
                catch (NullReferenceException)
                {
                    Debug.Log("Cant click");
                    throw;
                }
            }
        }
    }
}
