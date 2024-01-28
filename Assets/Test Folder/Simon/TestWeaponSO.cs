using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Test Weapon", menuName = "Weapon Scriptable Object/Melee Weapons")]
public class TestWeaponSO : ScriptableObject
{
    [SerializeField] private AnimatorOverrideController weaponAnimationToUse;

    [SerializeField] private float damage, knockbackMultiplier;

    [SerializeField] private CapsuleCollider2D hitbox;

    // TODO: Add particles.
    [SerializeField] private Sprite weaponSprite;
    [SerializeField] private float animationSpeedMultiplier;
    // Will the animation play reset animation after initial weapon swing.
    [SerializeField] private bool isWeaponAnimationResetable;
    // Only used if above bool is true.
    [SerializeField] private float resetMultiplier;

    #region Properties
    public AnimatorOverrideController AnimatorOverride { get => weaponAnimationToUse; }
    public float Damage { get => damage; }
    public float KnockBackMultiplier { get => knockbackMultiplier; }
    public CapsuleCollider2D Hitbox { get => hitbox; }
    public Sprite WeaponSprite { get => weaponSprite; }
    public float AnimationSpeed { get => animationSpeedMultiplier; }
    public bool IsWeaponResetable { get => isWeaponAnimationResetable; }
    public float ResetMultiplier { get => resetMultiplier; }
    #endregion
}
