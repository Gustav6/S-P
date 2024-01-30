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
        SpriteRenderer weaponSprite = controller.transform.GetChild(0).GetComponentInChildren<SpriteRenderer>();
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

        weaponSprite.sprite = newWeapon.WeaponSprite;

        if (oldParticleSystem != null)
            Destroy(oldParticleSystem);

        Instantiate(controller.CurrentWeapon.ParticleSystem, swingAnchor);
    }
}
