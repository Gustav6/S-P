using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

public class ShrimpEnemy : Enemy
{
    public override void TakeKnockback(Vector2 sourcePosition, float knockbackMultiplier, float stunDuration)
    {
        if (knockbackMultiplier == 0)
            return;

        // �ndra p� multiplier i olika enemies f�r olika knockback, f�lj detta som en outline.
        _enemyAttack.CanAttack(false);
        _attackController.LeaveMovement();
        _attackController.EnemyHit();

        Vector2 knockbackVector = ((Vector2)transform.position - sourcePosition).normalized;
        float multiplier = (4 + (KnockbackPercent / 100)) * knockbackMultiplier;

        StartCoroutine(SetEnemyVelocity(knockbackVector, multiplier, stunDuration));

        base.TakeKnockback(sourcePosition, knockbackMultiplier, stunDuration);
    }
}
