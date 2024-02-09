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

    #region Saved Data
    private PlayerData _data;
    StatBlock _mainStatBlock = new(1, 1, 1, 1, 1, 1);
    public WeaponSO CurrentWeapon;
    #endregion

    StatBlock _weaponStatBlock;
    StatBlock _abilityStatBlock;

    private PlayerMovement _playerMovement;

    [SerializeField] float _diStrength = 0.25f; // DI stands for direction input, used to reduce or enhance knockback when counteracting it with movement input.

    bool _isImmune = false;

    public float KnockbackPercent { get; set; }

    public PowerUp currentPowerUp { get; private set; }

    private void Start()
    {
        //_data = SaveSystem.Instance.LoadGameSave();

        // Call this when you want to change the player weapon n stuff.
        EquipmentManager.Instance.SwitchWeapon(CurrentWeapon);
    }

    public void SetLocalDataToSave(PlayerData data)
    {
        _mainStatBlock = data.MainStatBlock;
        CurrentWeapon = data.CurrentWeapon;
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

    public void NewAbilityEquipped(PowerUp powerUp)
    {
        currentPowerUp = powerUp;
    }

    public void ClearEquippedAbility()
    {
        if (currentPowerUp = null)
            return;

        Destroy(GetComponent<PowerUp>());
        currentPowerUp = null;
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
        // TODO: Add  GetStat(StatType.DamageResistance) back
        KnockbackPercent += damageAmount;
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

        float multiplier = (2 + (KnockbackPercent / 100)) * knockbackMultiplier;

        Vector2 knockbackVector = ((Vector2)transform.position - sourcePosition).normalized * multiplier;

        Vector2 diVector = input * (knockbackVector.magnitude * _diStrength);

        // TODO: Add GetStat(StatType.KnockbackResistance) back. Was causing some issues before.
        _playerMovement.rb.AddForce((knockbackVector + diVector), ForceMode2D.Impulse);

        Invoke(nameof(ResetKB), stunDuration);
    }

    void ResetKB()
    {
        _playerMovement.isGrounded = true;
        _isImmune = false;
    }
    #endregion
}
