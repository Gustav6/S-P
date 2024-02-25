using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActiveMenuManager : ActiveBaseManager
{
    public bool enableBlur;

    public override void Start()
    {
        base.Start();
    }

    public void EnableBlur(Blur blurScript)
    {
        blurScript.radius = 3.6f;
        blurScript.qualityIterations = 2;
        blurScript.filter = 2;
    }

    public void DisableBlur(Blur blurScript)
    {
        blurScript.radius = 0;
        blurScript.qualityIterations = 1;
        blurScript.filter = 0;
    }
}