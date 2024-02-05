using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IDamageable
{
    public float KnockbackPercent { get; set; }

    public virtual void TakeDamage(float damageAmount)
    {
        KnockbackPercent += damageAmount;
    }

    public void TakeKnockback(Vector2 sourcePosition, float knockbackMultiplier, float stunDuration);
}
