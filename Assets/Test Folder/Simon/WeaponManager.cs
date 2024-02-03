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
        // Index 1: Weapon sprite, 2: Flash sprite.
        SpriteRenderer weaponSpriteRenderer = controller.GetComponentsInChildren<SpriteRenderer>()[1];
        SpriteRenderer flashSpriteRenderer = controller.GetComponentsInChildren<SpriteRenderer>()[2];

        Transform swingEffect = controller.GetComponentsInChildren<Transform>(true)[7];
        GameObject oldParticleSystem;

        try
        {
            oldParticleSystem = swingEffect.GetChild(0).gameObject;
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

        Instantiate(controller.CurrentWeapon.ParticleSystem, swingEffect);
    }
}
