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
        Collider2D[] collidersInside = Physics2D.OverlapBoxAll(transform.position, transform.localScale, 0f);

        foreach (Collider2D collider in collidersInside)
        {
            // Manually invoke OnTriggerEnter2D for each object inside the trigger
            var damageable = collider.GetComponent<IDamageable>();

            if (damageable == null)
                return;

            if (_thisController == null)
            {
                if (collider.CompareTag(transform.parent.tag))
                    return;

                //  * PlayerStats.Instance.GetStat(StatType.DamageDealt)
                //  * PlayerStats.Instance.GetStat(StatType.KnockbackDealt)
                Attack(damageable, PlayerStats.Instance.CurrentWeapon.Damage,
                       PlayerStats.Instance.CurrentWeapon.KnockBackMultiplier, transform.position);
            }
            else
            {
                if (collider.CompareTag(_thisController.tag))
                    return;

                Attack(damageable, _thisController.Damage, _thisController.KnockbackMultiplier, _thisController.transform.position);
            }
        }
    }

    private void OnEnable()
    {
        Collider2D[] collidersInside = Physics2D.OverlapBoxAll(transform.position, transform.localScale, 0f);

        foreach (Collider2D collider in collidersInside)
        {
            // Manually invoke OnTriggerEnter2D for each object inside the trigger
            var damageable = collider.GetComponent<IDamageable>();

            if (damageable == null)
                return;

            if (_thisController == null)
            {
                Debug.Log("Testing");

                if (collider.CompareTag(transform.parent.tag))
                    return;

                //  * PlayerStats.Instance.GetStat(StatType.DamageDealt)
                //  * PlayerStats.Instance.GetStat(StatType.KnockbackDealt)
                Attack(damageable, PlayerStats.Instance.CurrentWeapon.Damage,
                       PlayerStats.Instance.CurrentWeapon.KnockBackMultiplier, transform.position);
            }
            else
            {
                if (collider.CompareTag(_thisController.tag))
                    return;

                Attack(damageable, _thisController.Damage, _thisController.KnockbackMultiplier, _thisController.transform.position);
            }
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
        // TODO: Change later.

        return 0.1f;
    }
}
