using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAttackController : MonoBehaviour
{
    Animator _anim;
    Enemy _enemy;

    private void Awake()
    {
        _anim = GetComponent<Animator>();
        _enemy = GetComponent<Enemy>();
    }

    public void EnterMovement()
    {
        _enemy._enemyAI.CanMove = true;
        _anim.SetBool("IsMoving", true);
    }

    public void LeaveMovement()
    {
        _enemy._enemyAI.CanMove = false;
        _anim.SetBool("IsMoving", false);
    }

    public void EnterAttackState()
    {
        _anim.SetFloat("AnimSpeed", 2f);
        _anim.SetBool("IsAttacking", true);
    }

    public void LeaveAttack()
    {
        _anim.SetBool("IsAttacking", false);
    }
}
