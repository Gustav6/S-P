using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Test Weapon", menuName = "Weapon Scriptable Object/Melee Weapons")]
public class WeaponSO : ScriptableObject
{
    [SerializeField] private AnimatorOverrideController weaponAnimationToUse;

    [SerializeField] private float damage, knockbackMultiplier;

    [SerializeField] private CapsuleCollider2D hitbox;

    // weaponFlashSprite: Leave null if you do not want weapon to flash after an attack animation.
    [SerializeField] private Sprite weaponSprite, weaponFlashSprite;
    [SerializeField] private GameObject particleSystem;

    [SerializeField] private float animationSpeedMultiplier;
    [SerializeField] private bool isWeaponAnimationResetable; // Will the animation play reset animation after initial weapon swing.
    [SerializeField] private float resetMultiplier; // Only used if above bool is true.

    #region Properties
    public AnimatorOverrideController AnimatorOverride { get => weaponAnimationToUse; }
    public float Damage { get => damage; }
    public float KnockBackMultiplier { get => knockbackMultiplier; }
    public CapsuleCollider2D Hitbox { get => hitbox; }
    public Sprite WeaponSprite { get => weaponSprite; }
    public Sprite WeaponFlashSprite { get => weaponFlashSprite; }
    public GameObject ParticleSystem { get => particleSystem; }
    public float AnimationSpeed { get => animationSpeedMultiplier; }
    public bool IsWeaponResetable { get => isWeaponAnimationResetable; }
    public float ResetMultiplier { get => resetMultiplier; }
    #endregion
}
