using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class HitboxTrigger : MonoBehaviour
{
    private Enemy _thisController;

    private const float _maxTime = 5;
    private float _timer;

    private void Awake()
    {
        _thisController = GetComponentInParent<Enemy>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        var damageable = collision.GetComponent<IDamageable>();

        if (damageable == null)
            return;

        if (_thisController != null)
        {
            if (collision.CompareTag(_thisController.tag))
                return;

            _thisController.ShouldCount = false;
            _thisController.ConsecutiveHits = 0;
            Attack(damageable, _thisController.Damage, _thisController.KnockbackMultiplier, _thisController.transform.position, CalculateStunTime(damageable.KnockbackPercent));
        }
        else if (transform.parent != null)
        {
            if (collision.CompareTag(transform.parent.tag))
                return;

            Attack(damageable, PlayerStats.Instance.CurrentWeapon.Damage * PlayerStats.Instance.GetStat(StatType.DamageDealt),
                   PlayerStats.Instance.CurrentWeapon.KnockBackMultiplier * PlayerStats.Instance.GetStat(StatType.KnockbackDealt), transform.position,
                   CalculateStunTime(damageable.KnockbackPercent, PlayerStats.Instance.CurrentWeapon.StunTime, damageable.ConsecutiveHits));
        }
        else
        {
            if (collision.CompareTag(tag))
                return;

            Attack(damageable, PlayerStats.Instance.CurrentWeapon.Damage * PlayerStats.Instance.GetStat(StatType.DamageDealt),
                   PlayerStats.Instance.CurrentWeapon.KnockBackMultiplier * PlayerStats.Instance.GetStat(StatType.KnockbackDealt), PlayerStats.Instance.transform.position,
                   CalculateStunTime(damageable.KnockbackPercent, PlayerStats.Instance.CurrentWeapon.StunTime, damageable.ConsecutiveHits));
        }

        if (gameObject.name.Contains("Water"))
            AudioManager.Instance.PlaySound("WaterGun");
    }

    public void Attack(IDamageable damageable, float damage, float knockbackMultiplier, Vector2 sourcePosition, float stunTime)
    {
        // TODO: Play SFX in take damage method.
        damageable.TakeDamage(damage);
        damageable.TakeKnockback(sourcePosition, knockbackMultiplier, stunTime);
    }

    /// <summary>
    /// Calculates how long an entity should be stunned for after getting hit.
    /// </summary>
    /// <param name="currentKnockbackPercent">The knockback percent of the entity getting hit</param>
    /// <returns></returns>
    private float CalculateStunTime(float currentKnockbackPercent)
    {
        float stunTime = currentKnockbackPercent / 250;
        stunTime = stunTime < 0.1f ? 0.1f : stunTime >= 0.5f ? 0.5f + ((stunTime - 0.5f) / 4) : stunTime;

        return stunTime;
    }

    /// <summary>
    /// Calculates how long an entity should be stunned for after getting hit, while having a base stun time and avoiding stun locking.
    /// </summary>
    /// <param name="currentKnockbackPercent">The knockback percent of the entity getting hit</param>
    /// <returns></returns>
    private float CalculateStunTime(float currentKnockbackPercent, float baseStunTime, int consecutiveHits)
    {
        float decreaseValuePerHit = 0.15f;

        float stunTime = CalculateStunTime(currentKnockbackPercent);
        stunTime = stunTime + baseStunTime;
        stunTime = stunTime - (decreaseValuePerHit * consecutiveHits) > 0 ? stunTime - (decreaseValuePerHit * consecutiveHits) : 0;

        return stunTime;
    }

    private void Update()
    {
        _timer += Time.deltaTime;

        if (_timer >= _maxTime)
            Destroy(gameObject);
    }
}
