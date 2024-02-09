using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAttackController : MonoBehaviour
{
    Animator _anim;
    Enemy _enemy;

    GameObject go;

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
        _anim.SetBool("IsAttacking", true);
    }

    public void LeaveAttack()
    {
        _anim.SetBool("IsAttacking", false);
    }

    public void AttackTest()
    {
        go = Instantiate(_enemy._enemyAttack.hitbox, _enemy._enemyAttack.hitboxSpawn.transform);
    }

    public void DespawnTest()
    {
        Debug.Log("ock");
        Destroy(go);
    }
}
