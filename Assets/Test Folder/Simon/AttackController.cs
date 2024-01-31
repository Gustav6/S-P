using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackController : MonoBehaviour
{
    public WeaponSO CurrentWeapon;

    // Animator on the Weapon Swing Anchor
    internal Animator weaponAnimator;

    private SpriteRenderer _flashSpriteRenderer;
    private Color flashOpaque = new Color(1, 1, 1, 0.6f), flashTransparent = new Color(1, 1, 1, 0);
    

    private bool _animationReadyToReset;
    public bool IsAnimationPlaying { get; private set; }
    
    private void Awake()
    {
        weaponAnimator = GetComponentInChildren<Animator>();
        _flashSpriteRenderer = GetComponentsInChildren<SpriteRenderer>()[1]; // Index 0: Weapon sprite, 1: Flash sprite, 2: Player sprite.
    }

    private void Start()
    {
        WeaponManager.Instance.SwitchWeapon(this, CurrentWeapon);
    }

    internal void PlayHitAnimation()
    {
        // TODO: Play SFX.

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
            _animationReadyToReset = true;
            weaponAnimator.SetTrigger("PlayReturn");
            weaponAnimator.SetFloat("s", CurrentWeapon.ResetMultiplier);
            return;
        }

        IsAnimationPlaying = false;

        // If there's an entity whos weapon shouldn't flash, like common enemies, just leave the flash sprite null in WeaponSO.
        if (_flashSpriteRenderer.sprite != null)
            StartCoroutine(FlashWeapon());

        if (CurrentWeapon.IsWeaponResetable)
        {
            _animationReadyToReset = false;
            weaponAnimator.SetFloat("s", CurrentWeapon.AnimationSpeed);
        }
    }

    /// <summary>
    /// Brefily turns the flash sprite of the weapon slightly opaque, before reverting.
    /// </summary>
    /// <returns></returns>
    private IEnumerator FlashWeapon()
    {
        _flashSpriteRenderer.color = flashOpaque;

        yield return new WaitForSeconds(0.1f);

        _flashSpriteRenderer.color = flashTransparent;
    }
}
