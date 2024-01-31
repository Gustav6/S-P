using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyControllerAttackTest : MonoBehaviour, IDamageable
{
    public float KnockbackPercent { get; set; }

    private AttackController _enemyAttackController;

    private void Awake()
    {
        _enemyAttackController = GetComponentInChildren<AttackController>();
    }

    public void TakeKnockback(float knockbackMultiplier)
    {
        Debug.Log($"Current enemy percent: {KnockbackPercent}%\nEnemy knockback as: {KnockbackPercent * knockbackMultiplier}");
    }

    private void Update()
    {
        if (Input.GetKey(KeyCode.P) && !_enemyAttackController.IsAnimationPlaying)
            _enemyAttackController.PlayHitAnimation(Vector2.one.normalized);
    }
}
