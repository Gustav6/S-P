using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAttack : MonoBehaviour, IDamageable
{
    public float KnockbackPercent { get; set; }

    private AttackController _enemyAttackController;

    private void Awake()
    {
        _enemyAttackController = GetComponentInChildren<AttackController>();
    }

    public void TakeKnockback(Vector2 sourcePosition, float knockbackMultiplier, float stunDuration)
    {
        Debug.Log($"Current enemy percent: {KnockbackPercent}% Enemy knockback as: {KnockbackPercent * knockbackMultiplier}\n" +
                  $"Hit from: {sourcePosition} and stunned for: {stunDuration}");
    }

    private void Update()
    {
        if (Input.GetKey(KeyCode.P) && !_enemyAttackController.IsAnimationPlaying)
            _enemyAttackController.PlayHitAnimation(Vector2.one.normalized);
    }
}
