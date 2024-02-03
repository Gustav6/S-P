using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Weapon", menuName = "ScriptableObject/Weapon")]
public class WeaponSO : ScriptableObject
{
    [SerializeField] private AnimatorOverrideController weaponAnimationToUse;

    [SerializeField] private Sprite weaponSprite;

    [SerializeField] private float animationSpeedMultiplier;
    [SerializeField] private float damage, knockbackMultiplier;

    #region SerializeField variables with tooltips
    [Tooltip("Used to determine how much force is added to an entity after swinging a weapon. Can be negative for a recoil effect.\n" +
            "Leave this variable, or the one below, at 0 and this effect will not occur.")]
    [SerializeField] private float weaponImpulseMultiplier;

    [Tooltip("Used to determine how long the force after a hit will last.\n" +
            "Leave this variable, or the one above, at 0 and this effect will not occur.")]
    [SerializeField] private float weaponImpulseTime;

    [Tooltip("Create a new GameObject and add a CapsuleCollider2D. Then determine its size and change its transform to where you want it to spawn.\n" +
            "To determine the transform, imagine that the hitbox will spawn at (0, 0) which will be inside the entity and move accordingly.")]
    [SerializeField] private CapsuleCollider2D hitbox;
    // TODO: Maybe change collider to gameobject to allow for all types of different colliders
    // and so that it can have a rigidbody which can impart velocity on spawn.

    [Tooltip("A sprite of the same shape as the weapon, but colored white. Will be used to create a flash effect on weapons.\n" +
            "Leave null for entities that you do not want this to happen on, such as very common enemies.")]
    [SerializeField] private Sprite weaponFlashSprite;

    [Tooltip("The particles you want to spawn behind the weapon, the transform on the prefab will affect spawn position on weapon.")]
    [SerializeField] private GameObject particleSystem;

    [Tooltip("Will the animation play a reset animation after initial weapon swing.")]
    [SerializeField] private bool isWeaponAnimationResetable;

    [Tooltip("Changes the speed of the return animation, only used if above bool is true.")]
    [SerializeField] private float resetMultiplier;
    #endregion

    #region Properties
    public AnimatorOverrideController AnimatorOverride { get => weaponAnimationToUse; }
    public Sprite WeaponSprite { get => weaponSprite; }
    public float Damage { get => damage; }
    public float KnockBackMultiplier { get => knockbackMultiplier; }
    public float AnimationSpeed { get => animationSpeedMultiplier; }
    public float ImpulseMultiplier { get => weaponImpulseMultiplier; }
    public float ImpulseEffectTime { get => weaponImpulseTime; }
    public CapsuleCollider2D Hitbox { get => hitbox; }
    public Sprite WeaponFlashSprite { get => weaponFlashSprite; }
    public GameObject ParticleSystem { get => particleSystem; }
    public bool IsWeaponResetable { get => isWeaponAnimationResetable; }
    public float ResetMultiplier { get => resetMultiplier; }
    #endregion
}
