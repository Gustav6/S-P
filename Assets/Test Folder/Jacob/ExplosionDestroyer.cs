using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExplosionDestroyer : MonoBehaviour
{
    Animator _anim;
    private void Awake()
    {
        _anim = GetComponent<Animator>();
    }

    public void Destroy()
    {
        Destroy(gameObject);
    }
}
