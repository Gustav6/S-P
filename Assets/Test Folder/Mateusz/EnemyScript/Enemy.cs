
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    public float _kbPower;

    [HideInInspector]
    public PlayerMovement _player;
    public EnemyAttack _enemyAttack;

    [HideInInspector]
    public Rigidbody2D _rb;

    [HideInInspector]
    public Animator _anim;
   
    private void Start()
    {
        _rb = GetComponent<Rigidbody2D>();

        _player = FindFirstObjectByType<PlayerMovement>();

        _enemyAttack = GetComponent<EnemyAttack>();
    }
}
    
  


