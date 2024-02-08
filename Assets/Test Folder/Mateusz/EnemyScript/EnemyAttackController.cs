using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAttackController : MonoBehaviour
{
    Enemy _enemy;

    SpriteRenderer _spriteRenderer;

    Rigidbody2D _rb;

    Animator _anim;

  
    private void Awake()
    {
        _enemy = FindFirstObjectByType<Enemy>();

        _rb = GetComponent<Rigidbody2D>();

        _anim = GetComponentInChildren<Animator>();
    }

    public void EnterAttackState()
    {
        _anim.SetBool("IsAttacking", true);
    }

    public void LeaveAttack()
    {
        _anim.SetBool("IsAttacking", false);
    }
}
