using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActiveBaseManager : MonoBehaviour
{
    public PrefabMoveDirection moveTowardsOnStart;
    public PrefabMoveDirection moveTowardsOnRemove;

    public virtual void Start()
    {
        
    }

    // Update is called once per frame
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
