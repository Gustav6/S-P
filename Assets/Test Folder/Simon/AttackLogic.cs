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

    // TODO: Add methods for sound effects and visual effects like camera shake for events.

    public void AnimationFinished()
    {
        animationComplete?.Invoke();
    }

    // Referenced in Unity Event.
    public void Attack(IDamageable damageable)
    {
        // IF I USE THE VARIABLE HERE IT BECOMES NULL FOR A FRAME OR SOMETHING.
        damageable.TakeDamage(GetComponent<AttackController>().CurrentWeapon.Damage);
        damageable.TakeKnockback(GetComponent<AttackController>().CurrentWeapon.KnockBackMultiplier);
    }
}
