using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UI : MonoBehaviour
{
    private UIManager uIManager;
    public UIManager UIManagerInstance { get { return uIManager; } }

    public bool activated = false;
    public Vector2 position;

    public void Start()
    {
        uIManager = GetComponentInParent<UIManager>();
    }
}
