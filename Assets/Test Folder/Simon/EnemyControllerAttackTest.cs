using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyControllerAttackTest : MonoBehaviour, IDamageable
{
    public float KnockbackPercent { get; set; }

    private AttackController _enemyController;

    private void Awake()
    {
        _enemyController = GetComponentInChildren<AttackController>();
    }

    public void TakeKnockback(float knockbackMultiplier)
    {
        Debug.Log($"Current enemy percent: {KnockbackPercent}%\nEnemy knockback as: {KnockbackPercent * knockbackMultiplier}");
    }

    private void Update()
    {
        if (Input.GetKey(KeyCode.P) && !_enemyController.IsAnimationPlaying)
        {
            _enemyController.PlayHitAnimation();
        }
    }
}
