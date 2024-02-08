using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EquipmentManager : MonoBehaviour
{
    #region Singleton Pattern
    public static EquipmentManager Instance;

    private void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
        {
            Destroy(gameObject);
        }
    }
    #endregion

    /// <summary>
    /// Call a chance to spawn a powerup at given position.
    /// Float describes the chance from 0 - 100 for the powerup to spawn.
    /// PowerUpTypes describes what type of powerup should spawn, the last element, Anything, randomly picks between all powerups.
    /// </summary>
    public Action<Vector2, float, PowerUpTypes> OnSpawnPowerUp { get; set; }

    private int _hitStopRequests;

    /// <summary>
    /// Switches the CurrentWeapon for the player.
    /// </summary>
    public void SwitchWeapon(WeaponSO newWeapon)
    {
        GameObject player = PlayerStats.Instance.gameObject;
        PlayerAnimationController controller = player.GetComponent<PlayerAnimationController>();

        // Index 3: Weapon sprite, 4: Flash sprite.
        SpriteRenderer weaponSpriteRenderer = player.GetComponentsInChildren<SpriteRenderer>()[3];
        SpriteRenderer flashSpriteRenderer = player.GetComponentsInChildren<SpriteRenderer>()[4];

        Transform swingEffect = weaponSpriteRenderer.transform.GetChild(0);
        GameObject oldParticleSystem;

        try
        {
            oldParticleSystem = swingEffect.GetChild(0).gameObject;
        }
        catch
        {
            oldParticleSystem = null;
        }

        PlayerStats.Instance.CurrentWeapon = newWeapon;

        // If either of these are null, the controller is not of a player but rather an enemy, which only need the weapon stats not the visual elements.
        if (weaponSpriteRenderer == null || flashSpriteRenderer == null || swingEffect == null)
            return;

        controller.weaponAnimator.SetFloat("s", newWeapon.AnimationSpeed);
        controller.weaponAnimator.runtimeAnimatorController = newWeapon.AnimatorOverride;

        weaponSpriteRenderer.sprite = newWeapon.WeaponSprite;
        flashSpriteRenderer.sprite = newWeapon.WeaponFlashSprite;

        if (oldParticleSystem != null)
            Destroy(oldParticleSystem);

        Instantiate(PlayerStats.Instance.CurrentWeapon.ParticleSystem, swingEffect);
    }

    /// <summary>
    /// Sends a request to stop the player from attacking.
    /// </summary>
    /// <param name="canHit">True sends a request to enable, false sends a request to disable.</param>
    public void ToggleHit(bool canHit)
    {
        if (canHit)
            _hitStopRequests--;
        else
            _hitStopRequests++;
    }

    /// <summary>
    /// Checks if the player can hit or not.
    /// </summary>
    public bool CanHit()
    {
        if (_hitStopRequests < 0)
            _hitStopRequests = 0;

        return _hitStopRequests == 0;
    }
}
