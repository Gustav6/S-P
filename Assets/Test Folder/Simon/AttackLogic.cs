using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class AttackLogic : MonoBehaviour
{
    [SerializeField] private UnityEvent animationComplete;

    private AttackController _attackController;

    private CapsuleCollider2D _hitbox;

    private void Awake()
    {
        _attackController = GetComponent<AttackController>();
    }

    public void SpawnHitbox()
    {
        _hitbox = Instantiate(_attackController.CurrentWeapon.Hitbox, transform.GetChild(0));
    }

    public void DespawnHitbox()
    {
        if (_hitbox != null)
            Destroy(_hitbox.gameObject);
    }

    public void AnimationFinished()
    {
        // TODO: Play SFX to show player can swing weapon again.

        animationComplete?.Invoke();
    }

    public static IEnumerator AddAttackBoost(Rigidbody2D rb, Vector2 targetDirection, float forceAmount, float forceActiveTime)
    {
        rb.AddForce(targetDirection * forceAmount, ForceMode2D.Impulse);

        yield return new WaitForSeconds(forceActiveTime);

        rb.velocity = Vector2.zero;
        //rb.angularVelocity = 0;
    }

    // Referenced in Unity Event.
    public void Attack(IDamageable damageable, AttackController attackController)
    {
        // If any cached variables are used within this method they will turn null, but remain the same value outside of method.
        // They will also not be null in the inspector. This did not happen before and just randomly began happening.

        // TODO: Play SFX in take damage method.
        damageable.TakeDamage(attackController.CurrentWeapon.Damage);
        damageable.TakeKnockback(attackController.CurrentWeapon.KnockBackMultiplier);
    }
}
