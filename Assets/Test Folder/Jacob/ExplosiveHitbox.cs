using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Modified version of HitboxTrigger.cs for explosive weaponry. Modified by Jacob.
public class ExplosiveHitbox : MonoBehaviour
{
    [SerializeField] GameObject _explosionPrefab;

    [SerializeField] float _explosionRadius;
    [SerializeField] LayerMask _enemyLayer;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player") || collision.CompareTag("WaveReward"))
            return;

        var enemyColliders = Physics2D.OverlapCircleAll(transform.position, _explosionRadius);
            //collision.GetComponent<IDamageable>();

        if (enemyColliders == null)
            return;

        foreach (Collider2D collider in enemyColliders)
        {
            if (collider.CompareTag("Player") || !collider.TryGetComponent(out IDamageable damageable))
            {
                continue;
            }

            Attack(damageable, PlayerStats.Instance.CurrentWeapon.Damage * PlayerStats.Instance.GetStat(StatType.DamageDealt),
                    PlayerStats.Instance.CurrentWeapon.KnockBackMultiplier * PlayerStats.Instance.GetStat(StatType.KnockbackDealt), transform.position,
                    CalculateStunTime(damageable.KnockbackPercent, PlayerStats.Instance.CurrentWeapon.StunTime, damageable.ConsecutiveHits), collider.transform);
        }

        Instantiate(_explosionPrefab, transform.position, Quaternion.identity);
        AudioManager.Instance.PlaySound("Explosion");
        Destroy(gameObject);
    }

    public void Attack(IDamageable damageable, float damage, float knockbackMultiplier, Vector2 sourcePosition, float stunTime, Transform enemyTransform)
    {
        // TODO: Play SFX in take damage method.
        damageable.TakeDamage(damage);
        damageable.TakeKnockback(sourcePosition, enemyTransform.position, knockbackMultiplier, stunTime);
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

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, _explosionRadius);
    }
}
