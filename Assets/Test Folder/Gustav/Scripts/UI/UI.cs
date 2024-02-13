using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class UI : MonoBehaviour
{
    [Header("UI variables")]
    public Vector2 position;
    public bool hovering;

    public bool IsDestroyed { get; set; }

    public virtual void Start()
    {
        IsDestroyed = false;
    }

    public virtual void Update()
    {
        if (UIManager.instance.Hovering(gameObject))
        {
            hovering = true;
            if (UIManager.instance != null)
            {
                UIManager.instance.CurrentUISelected = position;
            }
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
