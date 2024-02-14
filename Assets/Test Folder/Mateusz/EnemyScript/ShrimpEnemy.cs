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

        _enemyAttack.CanAttack(false);
        _attackController.LeaveMovement(false);
        _attackController.EnemyHit();
        _attackController.GroundEnemyHit();

        Vector2 knockbackVector = ((Vector2)Camera.main.ScreenToWorldPoint(Input.mousePosition) - sourcePosition).normalized;
        float multiplier = (4 + (KnockbackPercent / 100)) * knockbackMultiplier;

        StartCoroutine(SetEnemyVelocity(knockbackVector, multiplier, stunDuration));

        base.TakeKnockback(sourcePosition, knockbackMultiplier, stunDuration);
    }
}
