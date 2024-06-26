using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JellyfishEnemy : Enemy
{
    public override void TakeKnockback(Vector2 sourcePosition, float knockbackMultiplier, float stunDuration)
    {
        if (knockbackMultiplier == 0)
            return;

        _attackController.EnemyHit();
        _enemyAttack.CanAttack(false);
        _attackController.LeaveMovement(false);
        
        
        Vector2 knockbackVector = ((Vector2)Camera.main.ScreenToWorldPoint(Input.mousePosition) - sourcePosition).normalized;
        float multiplier = (10 + (KnockbackPercent / 100)) * knockbackMultiplier;

        StartCoroutine(SetEnemyVelocity(knockbackVector, multiplier, stunDuration));

        base.TakeKnockback(sourcePosition, knockbackMultiplier, stunDuration);
    }

    public override void TakeKnockback(Vector2 sourcePosition, Vector2 targetPosition, float knockbackMultiplier, float stunDuration)
    {
        if (knockbackMultiplier == 0)
            return;

        _attackController.EnemyHit();
        _enemyAttack.CanAttack(false);
        _attackController.LeaveMovement(false);


        Vector2 knockbackVector = (targetPosition - sourcePosition).normalized;
        float multiplier = (10 + (KnockbackPercent / 100)) * knockbackMultiplier;

        StartCoroutine(SetEnemyVelocity(knockbackVector, multiplier, stunDuration));

        base.TakeKnockback(sourcePosition, targetPosition, knockbackMultiplier, stunDuration);
    }
}
