using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JellyfishEnemy : Enemy
{
    public override void TakeKnockback(Vector2 sourcePosition, float knockbackMultiplier, float stunDuration)
    {
        if (knockbackMultiplier == 0)
            return;

        _enemyAttack.CanAttack(false);
        _attackController.LeaveMovement();
        _attackController.EnemyHit();

        Vector2 knockbackVector = ((Vector2)transform.position - sourcePosition).normalized;
        float multiplier = (10 + (KnockbackPercent / 100)) * knockbackMultiplier;

        StartCoroutine(SetEnemyVelocity(knockbackVector, multiplier, stunDuration));

        base.TakeKnockback(sourcePosition, knockbackMultiplier, stunDuration);

    }
}
