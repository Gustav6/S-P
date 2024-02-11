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

    public virtual void DeathCheck()
    {
        // TODO: Check if a majority of the entity's collider is in the water hitbox.
        // Call after taking knockback.
    }

    public virtual void Die()
    {
        // TODO: Play an animation and sfx before destroying.
        // Call in the death check.
    }
}
