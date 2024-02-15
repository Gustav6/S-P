using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Transition
{
    public delegate void ExecuteOnCompletion();
    public ExecuteOnCompletion executeOnCompletion;

    public bool isRemoved;
    public bool shouldBeRemoved = true;

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
            if (shouldBeRemoved)
            {
                isRemoved = true;
            }
        }
    }

    public virtual void SafetyNet()
    {

    }
}