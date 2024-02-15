using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class UI : MonoBehaviour
{
    [Header("UI variables")]
    public Vector2 position;
    public bool hovering;

    [Header("UI selected color")]
    public Color outlineSelected;
    public Color textSelected;

    [Header("UI deselected color")]
    public Color outlineDeselected;
    public Color textDeselected;

    [Header("UI selected scale")]
    public Vector3 selectedScale;

    public bool IsDestroyed { get; set; }

    public virtual void Start()
    {
        IsDestroyed = false;
    }

    public virtual void Update()
    {
        if (UIManager.Instance.Hovering(gameObject))
        {
            hovering = true;
            if (UIManager.Instance != null)
            {
                UIManager.Instance.CurrentUISelected = position;
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
