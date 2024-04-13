using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Simon
[CreateAssetMenu(fileName = "Weapon", menuName = "ScriptableObjects/Weapon")]
public class WeaponSO : ScriptableObject
{
    [Header("Weapon Basic Elements")]
    [Tooltip("Create a new GameObject and add a CapsuleCollider2D. Then determine its size and change its transform to where you want it to spawn.\n" +
            "To determine the transform, imagine that the hitbox will spawn at (0, 0) which will be inside the entity and move accordingly. Needs rigidbody for ranged weapons.")]
    [SerializeField] private GameObject hitbox;
    [SerializeField] private AnimatorOverrideController weaponAnimationToUse;

    [Space(10)]
    [Header("Logic Stats")]
    [Tooltip("Used to determine how much force is added to an entity after swinging a weapon. Can be negative for a recoil effect.\n" +
            "Leave this variable, or the one below, at 0 and this effect will not occur. This only works for the player.")]
    [SerializeField] private float weaponImpulseMultiplier;

    [Tooltip("Used to determine how long the force after a hit will last.\n" +
            "Leave this variable, or the one above, at 0 and this effect will not occur.")]
    [SerializeField] private float weaponImpulseTime;
    [SerializeField] private float damage, knockbackMultiplier;
    [Range(0.05f, 4)] [SerializeField] private float stunTime;

    [Space(5)]
    [Header("Animation Stats")]
    [SerializeField] private float animationSpeedMultiplier;

    [Tooltip("Will the animation play a reset animation after initial weapon swing.")]
    [SerializeField] private bool isWeaponAnimationResetable;

    [Tooltip("Changes the speed of the return animation, only used if above bool is true.")]
    [SerializeField] private float resetMultiplier;

    [Space(5)]
    [Header("Ranged Weapon Stats")]
    [Tooltip("If a weapon is ranged, use this variable to dictate the speed of the projectile.\n" +
            "If it is not ranged leave as 0.")]
    [Range(0, 1)] [SerializeField] private float projectileVelocity;

    [Space(10)]
    [Header("Visual Elements")]
    [SerializeField] private Sprite weaponSprite;

    [Tooltip("A sprite of the same shape as the weapon, but colored white. Will be used to create a flash effect on weapons.\n" +
            "Leave null for entities that you do not want this to happen on, such as very common enemies.")]
    [SerializeField] private Sprite weaponFlashSprite;

    [Tooltip("The particles you want to spawn behind the weapon, the transform on the prefab will affect spawn position on weapon.")]
    [SerializeField] private GameObject particleSystem;

    #region Properties
    public AnimatorOverrideController AnimatorOverride { get => weaponAnimationToUse; }
    public Sprite WeaponSprite { get => weaponSprite; }
    public float Damage { get => damage; }
    public float KnockBackMultiplier { get => knockbackMultiplier; }
    public float AnimationSpeed { get => animationSpeedMultiplier; }
    public float ImpulseMultiplier { get => weaponImpulseMultiplier; }
    public float ImpulseEffectTime { get => weaponImpulseTime; }
    public float HitboxVelocity { get => projectileVelocity; }
    public GameObject Hitbox { get => hitbox; }
    public Sprite WeaponFlashSprite { get => weaponFlashSprite; }
    public GameObject ParticleSystem { get => particleSystem; }
    public bool IsWeaponResetable { get => isWeaponAnimationResetable; }
    public float ResetMultiplier { get => resetMultiplier; }
    public float StunTime { get => stunTime; }
    #endregion
}
