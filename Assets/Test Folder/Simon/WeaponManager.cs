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
            Instance = this;
        else
            Destroy(gameObject);

        DontDestroyOnLoad(gameObject);
    }
    #endregion

    [SerializeField] private PlayerControllerAttackTest playerController;

    private SpriteRenderer _weaponSprite;

    private Transform _swingAnchor;

    private void Start()
    {
        _weaponSprite = playerController.transform.GetChild(0).GetComponentInChildren<SpriteRenderer>();
        _swingAnchor = playerController.transform.GetChild(0).GetChild(0).GetChild(0).GetChild(0);
    }

    /// <summary>
    /// Switches the CurrentWeapon for the player.
    /// </summary>
    public void SwitchWeapon(WeaponSO newWeapon)
    {
        playerController.CurrentWeapon = newWeapon;
        playerController.weaponAnimator.SetFloat("s", newWeapon.AnimationSpeed);
        playerController.weaponAnimator.runtimeAnimatorController = newWeapon.AnimatorOverride;

        _weaponSprite.sprite = newWeapon.WeaponSprite;

        Instantiate(playerController.CurrentWeapon.ParticleSystem, _swingAnchor);
    }
}
