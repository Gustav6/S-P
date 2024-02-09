
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour, IDamageable
{
    public float Damage, KnockbackMultiplier;

    [HideInInspector]
    public Rigidbody2D _rb;

    [HideInInspector]
    public EnemyAI _enemyAI;

    [HideInInspector]
    public EnemyAttackController _attackController;

    public float KnockbackPercent { get; set; }

    private void Awake()
    {
        _rb = GetComponent<Rigidbody2D>();
        _enemyAI = GetComponent<EnemyAI>();
        _attackController = GetComponent<EnemyAttackController>();
    }

    public virtual void TakeKnockback(Vector2 sourcePosition, float knockbackMultiplier, float stuntDuration)
    {
        _enemyAI.CanMove = false;
    }

    internal IEnumerator GiveEnemyMovement(float time)
    {
        yield return new WaitForSeconds(time);
        _enemyAI.CanMove = true;
    }
}
