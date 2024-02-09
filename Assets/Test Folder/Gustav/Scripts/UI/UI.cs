using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class UI : MonoBehaviour
{
    public Vector2 position;
    public bool hovering;

    public virtual void Start()
    {

    }

    public virtual void Update()
    {
        if (UIManager.instance.Hovering(gameObject))
        {
            hovering = true;
            UIManager.instance.CurrentUISelected = position;
        }
        else
        {
            hovering = false;
        }
    }

    public void LoadFunction(BaseStateManager manager)
    {
        manager.UIInstance = this;

        try
        {
            manager.OnStart();
        }
        catch (NullReferenceException)
        {
            Debug.Log("No state manager found");
        }
    }

    public void UpdateFunction(BaseStateManager manager)
    {
        if (manager != null)
        {
            manager.OnUpdate();
        }
    }
}
