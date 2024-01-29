using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Transition
{
    public delegate void ActionDelegate();
    public ActionDelegate actionDelegate;

    public bool isRemoved;

    public float timer;
    public float timerMax;

    public virtual void Start()
    {

    }

    public virtual void Update()
    {
        timer += Time.deltaTime;

        if (timer > timerMax)
        {
            isRemoved = true;
        }
    }
}