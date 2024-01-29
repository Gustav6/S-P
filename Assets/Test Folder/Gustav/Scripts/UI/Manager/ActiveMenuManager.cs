using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActiveMenuManager : MonoBehaviour
{
    public MoveTowards whereWillPrefabMove;

    void Start()
    {
    }
}

public enum MoveTowards
{
    Left, 
    Right,
    None
}