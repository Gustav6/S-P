using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class HitboxTrigger : MonoBehaviour
{
    private Enemy _thisController;

    private void Awake()
    {
        _thisController = GetComponentInParent<Enemy>();
    }

    private void Start()
    {
        Debug.Log($"Knockback: {PlayerStats.Instance.GetStat(StatType.KnockbackDealt)}\nDamage: {PlayerStats.Instance.GetStat(StatType.DamageDealt)}\n" +
                  $"Knockback resistance: {PlayerStats.Instance.GetStat(StatType.KnockbackResistance)}\nDamage resistance: {PlayerStats.Instance.GetStat(StatType.KnockbackResistance)}");
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        var damageable = collision.GetComponent<IDamageable>();

        if (damageable == null)
            return;

        if (_thisController == null)
        {
            if (collision.CompareTag(transform.parent.tag))
                return;

            Attack(damageable, PlayerStats.Instance.CurrentWeapon.Damage * PlayerStats.Instance.GetStat(StatType.DamageDealt),
                   PlayerStats.Instance.CurrentWeapon.KnockBackMultiplier * PlayerStats.Instance.GetStat(StatType.KnockbackDealt), transform.position);
        }
        else
        {
            if (collision.CompareTag(_thisController.tag))
                return;

            Attack(damageable, _thisController.Damage, _thisController.KnockbackMultiplier, _thisController.transform.position);
        }
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
        float stunTime = currentKnockbackPercent / 250;
        stunTime = stunTime >= 0.5f ? 0.5f + ((stunTime - 0.5f) / 4) : stunTime;

        return stunTime;
    }
}
