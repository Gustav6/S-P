using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponManager : MonoBehaviour
{
    #region Singleton Pattern
    public static WeaponManager instance;

    private void Awake()
    {
        if (instance == null)
            instance = this;
        else
            Destroy(gameObject);

        DontDestroyOnLoad(gameObject);
    }
    #endregion

    [SerializeField] private PlayerControllerAttackTest playerController;
    private static PlayerControllerAttackTest _player;

    private static SpriteRenderer _weaponSprite;

    private void Start()
    {
        _weaponSprite = playerController.transform.GetChild(0).GetComponentInChildren<SpriteRenderer>();
        _player = playerController;
    }

    /// <summary>
    /// Switches the CurrentWeapon for the player.
    /// </summary>
    public static void SwitchWeapon(TestWeaponSO newWeapon)
    {
        _player.CurrentWeapon = newWeapon;
        _player.weaponAnimator.SetFloat("s", newWeapon.AnimationSpeed);
        _player.weaponAnimator.runtimeAnimatorController = newWeapon.AnimatorOverride;

        _weaponSprite.sprite = newWeapon.WeaponSprite;
    }
}
