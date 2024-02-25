using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActiveBaseManager : MonoBehaviour
{
    public virtual void Start()
    {

    }

    public virtual void Update()
    {

    }
}

public enum PrefabMoveDirection
{
    Left,
    Right,
    Up,
    Down,
    None,
}
