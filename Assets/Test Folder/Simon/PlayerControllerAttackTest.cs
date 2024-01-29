using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerControllerAttackTest : MonoBehaviour
{
    public WeaponSO CurrentWeapon;

    // Animator on the Weapon Swing Anchor
    internal Animator weaponAnimator;

    private Transform _weaponRotationPoint;

    private bool _animationReadyToReset, _isAnimationPlaying;

    private void Awake()
    {
        weaponAnimator = GetComponentInChildren<Animator>();
        _weaponRotationPoint = transform.GetChild(0);
    }

    private void Start()
    {
        // Remove later.
        WeaponManager.SwitchWeapon(CurrentWeapon);
    }

    private void Update()
    {
        if (Input.GetKey(KeyCode.Space) && !_isAnimationPlaying)
        {
            _isAnimationPlaying = true;
            PlayHitAnimation();
        }

        TurnToMouse();
    }

    /// <summary>
    /// Stop the players attack.
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

        _isAnimationPlaying = false;

        if (CurrentWeapon.IsWeaponResetable)
        {
            _animationReadyToReset = false;
            weaponAnimator.SetFloat("s", CurrentWeapon.AnimationSpeed);
        }
    }

    /// <summary>
    /// For testing, remove later.
    /// </summary>
    private void TurnToMouse()
    {
        Vector3 mousePosition = Input.mousePosition;
        Vector3 targetPosition = Camera.main.ScreenToWorldPoint(mousePosition);

        Vector3 direction = targetPosition - _weaponRotationPoint.position;
        float angle = Mathf.Atan2(direction.x, direction.y) * -Mathf.Rad2Deg;
        _weaponRotationPoint.rotation = Quaternion.AngleAxis(angle, Vector3.forward);
    }

    private void PlayHitAnimation()
    {
        weaponAnimator.SetTrigger("PlayHit");
        weaponAnimator.SetFloat("s", CurrentWeapon.AnimationSpeed);
    }

    // Referenced in Unity Event.
    public void Attack(IDamageable damageable)
    {
        damageable.TakeDamage(CurrentWeapon.Damage);
        damageable.TakeKnockback();
    }
}
