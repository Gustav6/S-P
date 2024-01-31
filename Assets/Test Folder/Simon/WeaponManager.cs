using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponManager : MonoBehaviour
{
    #region Singleton Pattern
    public static WeaponManager Instance;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }
    #endregion

    /// <summary>
    /// Switches the CurrentWeapon for the player.
    /// </summary>
    public void SwitchWeapon(AttackController controller, WeaponSO newWeapon)
    {
        // Index 0: Weapon sprite, 1: Flash sprite, 2: Player sprite.
        SpriteRenderer weaponSpriteRenderer = controller.GetComponentsInChildren<SpriteRenderer>()[0];
        SpriteRenderer flashSpriteRenderer = controller.GetComponentsInChildren<SpriteRenderer>()[1];

        // Do not worry about it.
        Transform swingAnchor = controller.transform.GetChild(0).GetChild(0).GetChild(0).GetChild(0);
        GameObject oldParticleSystem;

        try
        {
            // Don't worry about it.
            oldParticleSystem = controller.transform.GetChild(0).GetChild(0).GetChild(0).GetChild(0).GetChild(0).gameObject;
        }
        catch
        {
            oldParticleSystem = null;
        }

        controller.CurrentWeapon = newWeapon;
        controller.weaponAnimator.SetFloat("s", newWeapon.AnimationSpeed);
        controller.weaponAnimator.runtimeAnimatorController = newWeapon.AnimatorOverride;

        weaponSpriteRenderer.sprite = newWeapon.WeaponSprite;
        flashSpriteRenderer.sprite = newWeapon.WeaponFlashSprite;

        if (oldParticleSystem != null)
            Destroy(oldParticleSystem);

        Instantiate(controller.CurrentWeapon.ParticleSystem, swingAnchor);
    }
}
