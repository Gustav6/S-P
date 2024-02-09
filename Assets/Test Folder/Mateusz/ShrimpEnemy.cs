using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShrimpEnemy : Enemy
{
    public override void TakeKnockback(Vector2 sourcePosition, float knockbackMultiplier, float stuntDuration)
    {
        // Ändra på multiplier i olika enemies för olika knockback, följ detta som en outline.

        _enemyAttack.CanAttack(false);
        _attackController.LeaveMovement();

        Vector2 knockbackVector = ((Vector2)transform.position - sourcePosition).normalized;
        float multiplier = (4 + (KnockbackPercent / 100)) * knockbackMultiplier;

        _rb.velocity = (knockbackVector * multiplier);

        base.TakeKnockback(sourcePosition, knockbackMultiplier, stuntDuration);
    }
}
