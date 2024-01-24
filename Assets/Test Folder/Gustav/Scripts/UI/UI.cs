using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UI : MonoBehaviour
{
    [SerializeField] private UIManager manager;
    public UIManager Manager { get { return manager; } }

    public bool activated = false;
    public Vector2 position;

    public delegate void ActionDelegate();
    public ActionDelegate actionDelegate;

    public void Start()
    {
        manager = GetComponentInParent<UIManager>();
    }
}
