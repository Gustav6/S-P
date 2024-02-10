using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WhaleEnemy : Enemy
{
    public override void TakeKnockback(Vector2 sourcePosition, float knockbackMultiplier, float stuntDuration)
    {
        _enemyAttack.CanAttack(false);
        _attackController.LeaveMovement();

        Vector2 knockbackVector = ((Vector2)transform.position - sourcePosition).normalized;
        float multiplier = (2 + (KnockbackPercent / 100)) * knockbackMultiplier;

        Debug.Log(knockbackVector * multiplier);
        _rb.velocity = (knockbackVector * multiplier);

        base.TakeKnockback(sourcePosition, knockbackMultiplier, stuntDuration);
    }
}
