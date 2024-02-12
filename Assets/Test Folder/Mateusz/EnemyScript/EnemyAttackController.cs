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
        _anim.SetBool("IsHit", false);
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
        _anim.SetFloat("AnimSpeed", 1.5f);
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

    public void EnemyHit()
    {
        if (go != null)
            Destroy(go);
        _anim.SetBool("IsHit", true);
    }


    public void DespawnHitbox()
    {
        Destroy(go);
    }
}
