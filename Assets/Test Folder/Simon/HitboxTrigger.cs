using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class HitboxTrigger : MonoBehaviour
{
    // TODO: Change for enemy controllr if null do the bellow if not then use enemy controller stat.
    private AttackController _parentController;

    private void Awake()
    {
        _parentController = GetComponentInParent<AttackController>();
    }

    private void OnTriggerEnter2D(Collider2D triggerInfo)
    {
        var damageable = triggerInfo.GetComponent<IDamageable>();

        if (damageable == null || triggerInfo.CompareTag(_parentController.tag))
            return;

        if (_parentController != null)
            Attack(damageable, PlayerStats.Instance.CurrentWeapon.Damage * PlayerStats.Instance.GetStat(StatType.DamageDealt),
                   PlayerStats.Instance.CurrentWeapon.KnockBackMultiplier * PlayerStats.Instance.GetStat(StatType.KnockbackDealt), transform.position);
    }

    public void Attack(IDamageable damageable, float damage, float knockbackMultiplier, Vector2 sourcePosition)
    {
        // TODO: Play SFX in take damage method.
        damageable.TakeDamage(damage);
        damageable.TakeKnockback(sourcePosition, knockbackMultiplier, CalculateStunTime(damageable.KnockbackPercent));
    }

    /// <summary>
    /// Calculates how long an entity should be stunned for after getting hit.
    /// </summary>
    /// <param name="currentKnockbackPercent">The knockback percent of the entity getting hit</param>
    /// <returns></returns>
    private float CalculateStunTime(float currentKnockbackPercent)
    {
        // TODO: Change later.

        return currentKnockbackPercent / 100;
    }
}
