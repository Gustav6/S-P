using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAttackController : MonoBehaviour
{
    Enemy _enemy;
    PlayerStats _player;

    SpriteRenderer _spriteRenderer;

    Rigidbody2D _rb;

    Animator _anim;

  
    private void Awake()
    {
        _enemy = FindFirstObjectByType<Enemy>();
        _player = FindFirstObjectByType<PlayerStats>();

        _rb = GetComponent<Rigidbody2D>();

        _anim = GetComponentInChildren<Animator>();
    }

   internal void EnterAttackState()
    {
        _anim.SetTrigger("Attack ");
        _player.TakeKnockback(Vector2.zero, 2, 0.25f);
        Debug.Log("Is Attacking");
    }

    public void LeaveAttack()
    {

    }
}
