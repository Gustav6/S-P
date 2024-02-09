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
        _enemy._enemyAI.IsNotGettingHit = true;
        _anim.SetBool("IsMoving", true);
    }

    public void LeaveMovement()
    {
        _enemy._enemyAI.CanMove = false;
        _enemy._enemyAI.IsNotGettingHit = false;
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

    public void EnemyAttack()
    {
        go = Instantiate(_enemy._enemyAttack.hitbox, _enemy._enemyAttack.hitboxSpawn.transform);
        go.transform.localPosition *= new Vector2(Mathf.Sign(transform.localScale.x), 1);
    }

    public void DespawnHitbox()
    {
        Destroy(go);
    }
}
