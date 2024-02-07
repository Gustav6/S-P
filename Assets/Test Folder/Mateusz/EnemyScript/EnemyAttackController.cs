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

   internal void EnterAttackState()
    {
       
            WeaponManager.Instance.ToggleHit(true);
            _anim.SetBool("isAttacking", true);
            Debug.Log("Is Attacking");
       
    }

    public void LeaveAttack()
    {

    }
}
