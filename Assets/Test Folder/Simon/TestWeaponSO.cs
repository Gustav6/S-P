using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Test Weapon", menuName = "Weapon Scriptable Object/Melee Weapons")]
public class TestWeaponSO : ScriptableObject
{
    [SerializeField] private AnimatorOverrideController weaponAnimationToUse;

    [SerializeField] private float damage, knockbackMultiplier;

    [SerializeField] private CircleCollider2D hitbox;
    [SerializeField] private Vector2 hitboxToPlayerOffset;

    // TODO: Add particles.
    [SerializeField] private Texture2D weaponTexture;
    [SerializeField] private float animationSpeedMultiplier;
    // Will the animation play reset animation after initial weapon swing.
    [SerializeField] private bool isWeaponAnimationResetable;

    #region Properties
    public AnimatorOverrideController AnimatorOverride { get => weaponAnimationToUse; }
    public float Damage { get => damage; }
    public float KnockBackMultiplier { get => knockbackMultiplier; }
    public CircleCollider2D Hitbox { get => hitbox; }
    public Vector2 HitboxToPlayerOffset { get => hitboxToPlayerOffset; }
    public Texture2D WeaponTexture { get => weaponTexture; }
    public float AnimationSpeed { get => animationSpeedMultiplier; }
    public bool IsWeaponResetable { get => isWeaponAnimationResetable; }
    #endregion
}
