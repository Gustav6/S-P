using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class PlayerAnimationController : MonoBehaviour
{
    private PlayerMovement _player;
    internal Animator weaponAnimator;

    private SpriteRenderer _flashSpriteRenderer;
    private Color flashOpaque = new Color(1, 1, 1, 0.6f), flashTransparent = new Color(1, 1, 1, 0);

    private Rigidbody2D _rb;

    private Coroutine _attackForceCoroutine;

    private bool _animationReadyToReset;
    internal bool inAnimation { get; private set; }

    private void Awake()
    {
        weaponAnimator = GetComponentInChildren<Animator>();
        _flashSpriteRenderer = GetComponentsInChildren<SpriteRenderer>()[4]; // Index 3: Weapon sprite, 4: Flash sprite.
        _rb = GetComponent<Rigidbody2D>();

        _player = GetComponent<PlayerMovement>();
    }

    /// <summary>
    /// Plays the attack animation, which will then begin calling the attack logic.
    /// </summary>
    /// <param name="attackForceDirection">The direction the entity should get a force added towards when attacking</param>
    /// <returns></returns>
    internal void PlayHitAnimation(Vector2 attackForceDirection)
    {
        // TODO: Play SFX.

        inAnimation = true;

        weaponAnimator.SetTrigger("PlayHit");
        weaponAnimator.SetFloat("s", PlayerStats.Instance.CurrentWeapon.AnimationSpeed * PlayerStats.Instance.GetStat(StatType.AttackSpeed));

        float impulseMultiplier = PlayerStats.Instance.CurrentWeapon.ImpulseMultiplier;
        float impulseTime = PlayerStats.Instance.CurrentWeapon.ImpulseEffectTime;

        if (_player == null || impulseMultiplier == 0 || impulseTime == 0)
            return;

        if (_attackForceCoroutine != null)
            StopCoroutine(_attackForceCoroutine);

        _attackForceCoroutine = StartCoroutine(AttackLogic.AddBoost(_player, _rb, attackForceDirection, impulseMultiplier, impulseTime));
    }

    /// <summary>
    /// Stop the current attack.
    /// Used to leave the state when an attack animation is finished playing.
    /// < /summary>
    public void LeaveAttackState()
    {
        if (PlayerStats.Instance.CurrentWeapon.IsWeaponResetable && !_animationReadyToReset)
        {
            _animationReadyToReset = true;
            weaponAnimator.SetTrigger("PlayReturn");
            weaponAnimator.SetFloat("s", PlayerStats.Instance.CurrentWeapon.ResetMultiplier * PlayerStats.Instance.GetStat(StatType.AttackSpeed));
            return;
        }

        inAnimation = false;

        // If there's an entity whos weapon shouldn't flash, like common enemies, just leave the flash sprite null in WeaponSO.
        if (_flashSpriteRenderer.sprite != null)
            StartCoroutine(FlashWeapon());

        if (PlayerStats.Instance.CurrentWeapon.IsWeaponResetable)
        {
            _animationReadyToReset = false;
            weaponAnimator.SetFloat("s", PlayerStats.Instance.CurrentWeapon.AnimationSpeed * PlayerStats.Instance.GetStat(StatType.AttackSpeed));
        }
    }

    /// <summary>
    /// Brefily turns the flash sprite of the weapon slightly opaque, before reverting.
    /// </summary>
    private IEnumerator FlashWeapon()
    {
        _flashSpriteRenderer.color = flashOpaque;

        yield return new WaitForSeconds(0.1f);

        _flashSpriteRenderer.color = flashTransparent;
    }
}
