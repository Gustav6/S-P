using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class HitboxTrigger : MonoBehaviour
{
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

        Attack(damageable, _parentController);
    }

    public void Attack(IDamageable damageable, AttackController attackController)
    {
        // If any cached variables are used within this method they will turn null, but remain the same value outside of method.
        // They will also not be null in the inspector. This did not happen before and just randomly began happening.

        // TODO: Play SFX in take damage method.
        damageable.TakeDamage(attackController.CurrentWeapon.Damage);
        damageable.TakeKnockback(attackController.transform.position, attackController.CurrentWeapon.KnockBackMultiplier, CalculateStunTime(damageable.KnockbackPercent));
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
