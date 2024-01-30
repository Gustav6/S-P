using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackController : MonoBehaviour
{
    public WeaponSO CurrentWeapon;

    // Animator on the Weapon Swing Anchor
    internal Animator weaponAnimator;

    // Remove later.
    internal Transform _weaponRotationPoint;

    private bool _animationReadyToReset;
    public bool IsAnimationPlaying { get; private set; }

    private void Awake()
    {
        weaponAnimator = GetComponentInChildren<Animator>();
        _weaponRotationPoint = transform.GetChild(0);
    }

    private void Start()
    {
        WeaponManager.Instance.SwitchWeapon(this, CurrentWeapon);
    }

    internal void PlayHitAnimation()
    {
        IsAnimationPlaying = true;

        weaponAnimator.SetTrigger("PlayHit");
        weaponAnimator.SetFloat("s", CurrentWeapon.AnimationSpeed);
    }

    /// <summary>
    /// Stop the current attack.
    /// Used to leave the state when an attack animation is finished playing.
    /// </summary>
    public void LeaveAttackState()
    {
        if (CurrentWeapon.IsWeaponResetable && !_animationReadyToReset)
        {
            weaponAnimator.SetTrigger("PlayHit");
            _animationReadyToReset = true;
            weaponAnimator.SetFloat("s", CurrentWeapon.ResetMultiplier);
            return;
        }

        IsAnimationPlaying = false;

        if (CurrentWeapon.IsWeaponResetable)
        {
            _animationReadyToReset = false;
            weaponAnimator.SetFloat("s", CurrentWeapon.AnimationSpeed);
        }
    }

    // Referenced in Unity Event.
    public void Attack(IDamageable damageable)
    {
        damageable.TakeDamage(CurrentWeapon.Damage);
        damageable.TakeKnockback(CurrentWeapon.KnockBackMultiplier);
    }
}
