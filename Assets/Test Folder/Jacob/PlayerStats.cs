using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStats : MonoBehaviour, IDamageable
{
    #region Singleton

    public static PlayerStats Instance;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Debug.LogError("More than one instance of PlayerStats found on " + gameObject + ", Destroying instance");
            Destroy(this);
        }

        _playerMovement = GetComponent<PlayerMovement>();
    }

    #endregion

    StatBlock _mainStatBlock = new(1, 1, 1, 1, 1, 1);

    StatBlock _weaponStatBlock;
    StatBlock _abilityStatBlock;

    private PlayerMovement _playerMovement;

    public WeaponSO CurrentWeapon;

    [SerializeField] float _diStrength = 0.25f; // DI stands for direction input, used to reduce or enhance knockback when counteracting it with movement input.

    public float KnockbackPercent { get; set; }

    bool _isImmune = false;

    private void Start()
    {
        // Call this when you want to change the player weapon n stuff.
        WeaponManager.Instance.SwitchWeapon(CurrentWeapon);
    }

    public float GetStat(StatType stat)
    {
        return (_mainStatBlock * _weaponStatBlock * _abilityStatBlock).GetValue((int)stat).Value;
    }

    public void AddStatModifier(StatBlock statChange)
    {
        _mainStatBlock *= statChange;
    }

    public void NewWeaponEquipped(StatBlock newWeaponStatBlock)
    {
        _weaponStatBlock = newWeaponStatBlock;
    }

    public void NewAbilityEquipped()
    {

    }

    public void ActivateAbilityStats(StatBlock stats)
    {
        _abilityStatBlock = stats;
    }

    public void DeActivateAbilityStats()
    {
        _abilityStatBlock = new(1, 1, 1, 1, 1, 1);
    }

    #region Damage and Knockback
    void IDamageable.TakeDamage(float damageAmount)
    {
        KnockbackPercent += damageAmount / GetStat(StatType.DamageResistance);
    }

    /// <summary>
    /// Applies knockback to player and prevents them from moving for a specified time.
    /// </summary>
    /// <param name="sourcePosition">The position of the entity dealing the knockback.</param>
    /// <param name="knockbackMultiplier">Intensity of knockback.</param>
    /// <param name="stunDuration">Duration in seconds that movement is prevented after being hit.</param>
    public void TakeKnockback(Vector2 sourcePosition, float knockbackMultiplier, float stunDuration)
    {
        if (_isImmune)
            return;

        _playerMovement.isGrounded = false;
        _isImmune = true;

        Vector2 input = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical")).normalized;

        Vector2 knockbackVector = ((Vector2)transform.position - sourcePosition).normalized * knockbackMultiplier;

        Vector2 diVector = input * (knockbackVector.magnitude * _diStrength);

        _playerMovement.rb.AddForce((knockbackVector + diVector) / GetStat(StatType.KnockbackResistance), ForceMode2D.Impulse);

        Invoke(nameof(ResetKB), stunDuration);
    }

    void ResetKB()
    {
        _playerMovement.isGrounded = true;
        _isImmune = false;
    }
    #endregion
}
