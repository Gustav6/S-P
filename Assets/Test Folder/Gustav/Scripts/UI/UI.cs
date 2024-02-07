using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class UI : MonoBehaviour
{
    public Vector2 position;

    public virtual void Start()
    {

    }

    public virtual void Update()
    {

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
