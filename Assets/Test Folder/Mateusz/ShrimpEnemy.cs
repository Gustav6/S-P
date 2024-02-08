using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShrimpEnemy : Enemy
{
    public override void TakeKnockback(Vector2 sourcePosition, float knockbackMultiplier, float stuntDuration)
    {
        Vector2 knockbackVector = ((Vector2)transform.position - sourcePosition).normalized;
        float multiplier = (4 + (KnockbackPercent / 100)) * knockbackMultiplier;

        base.TakeKnockback(sourcePosition, knockbackMultiplier, stuntDuration);

        _rb.AddForce(knockbackVector * multiplier, ForceMode2D.Impulse);

        StartCoroutine(GiveEnemyMovement(stuntDuration));
    }
}
