using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Test Weapon", menuName = "Weapon Scriptable Object/Melee Weapons")]
public class TestWeaponSO : ScriptableObject
{
    // TODO: Add weapon sprite and animation to use depending on weapon varity

    [SerializeField] private CircleCollider2D hitbox;
    [SerializeField] private Vector2 hitboxToPlayerOffset;

    [SerializeField] private float startUp, active, windDown;

    [SerializeField] private float damage;

    #region Properties
    public CircleCollider2D Hitbox { get => hitbox; }
    public Vector2 HitboxToPlayerOffset { get => hitboxToPlayerOffset; }
    public float StartUp { get => startUp; }
    public float Active { get => active; } 
    public float WindDown { get => windDown; }
    public float Damage { get => damage; }
    #endregion
}
