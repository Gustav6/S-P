using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActiveMenuManager : MonoBehaviour
{
    private UIManager manager;
    void Start()
    {
        manager = GetComponentInParent<UIManager>();

        if (manager != null)
        {
            manager.SetTransitioning(false);
        }
    }
}
