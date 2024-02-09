using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShrimpEnemy : Enemy
{
    public override void TakeKnockback(Vector2 sourcePosition, float knockbackMultiplier, float stuntDuration)
    {
        // �ndra p� multiplier i olika enemies f�r olika knockback, f�lj detta som en outline.

        _enemyAttack.CanAttack(false);
        _attackController.LeaveMovement();

        Vector2 knockbackVector = ((Vector2)transform.position - sourcePosition).normalized;
        float multiplier = (4 + (KnockbackPercent / 100)) * knockbackMultiplier;

        _rb.velocity = (knockbackVector * multiplier);

        base.TakeKnockback(sourcePosition, knockbackMultiplier, stuntDuration);
    }
}
