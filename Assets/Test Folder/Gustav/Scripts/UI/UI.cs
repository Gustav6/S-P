using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class UI : MonoBehaviour
{
    [Header("UI variables")]
    public Vector2 position;
    public bool hovering;
    public bool isInteractable;

    [Header("UI selected color")]
    public Color outlineSelected;
    public Color textSelected;

    [Header("UI deselected color")]
    public Color outlineDeselected;
    public Color textDeselected;

    [Header("UI selected scale")]
    public Vector3 selectedScale;

    public virtual void Start()
    {

    }

    public virtual void Update()
    {
        if (GetComponent<Collider2D>().OverlapPoint(Input.mousePosition))
        {
            hovering = true;

            if (isInteractable)
            {
                UIStateManager.Instance.CurrentUISelected = position;
            }
        }
        else if (hovering)
        {
            hovering = false;
        }
    }

    public void LoadFunction(BaseStateManager manager)
    {
        manager.UIInstance = this;

        if (manager.UIInstance != null)
        {
            manager.OnStart();
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
