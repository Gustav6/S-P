using NUnit.Framework.Constraints;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EquipmentManager : MonoBehaviour
{
    #region Singleton Pattern
    public static EquipmentManager Instance;

    private void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
        {
            Destroy(gameObject);
        }

        SetPowerUpCanSpawn(true);
    }
    #endregion

    /// <summary>
    /// Call a chance to spawn a powerup at given position.
    /// Float describes the chance from 0 - 100 for the powerup to spawn.
    /// PowerUpTypes describes what type of powerup should spawn, the last element, Anything, randomly picks between all powerups.
    /// </summary>
    public Action<Vector2, float, PowerUpTypes> OnSpawnPowerUp { get; set; }
    
    public Action OnPowerUpEquipped { get; set; }
    public Action OnPowerUpUsed { get; set; }

    public Action PlayerTookDamage { get; set; }

    [Tooltip("0 = Dash, 1 = Haste, 2 = Tank")]
    [SerializeField] private List<Sprite> _powerupSprites = new List<Sprite>();

    private bool _canPlayerAttack = true;

    public bool CanSpawnPowerUps { get; private set; }

    GameObject _spawnedPowerup;

    /// <summary>
    /// Switches the CurrentWeapon for the player.
    /// </summary>
    public void SwitchWeapon(WeaponSO newWeapon)
    {
        GameObject player = PlayerStats.Instance.gameObject;
        PlayerAnimationController controller = player.GetComponent<PlayerAnimationController>();

        // Index 3: Weapon sprite, 4: Flash sprite.
        SpriteRenderer weaponSpriteRenderer = player.GetComponentsInChildren<SpriteRenderer>()[3];
        SpriteRenderer flashSpriteRenderer = player.GetComponentsInChildren<SpriteRenderer>()[4];

        Transform swingEffect = weaponSpriteRenderer.transform.GetChild(0);
        GameObject oldParticleSystem;

        try
        {
            oldParticleSystem = swingEffect.GetChild(0).gameObject;
        }
        catch
        {
            oldParticleSystem = null;
        }

        PlayerStats.Instance.CurrentWeapon = newWeapon;

        controller.weaponAnimator.SetFloat("s", newWeapon.AnimationSpeed);
        controller.weaponAnimator.runtimeAnimatorController = newWeapon.AnimatorOverride;

        weaponSpriteRenderer.sprite = newWeapon.WeaponSprite;
        flashSpriteRenderer.sprite = newWeapon.WeaponFlashSprite;

        if (oldParticleSystem != null)
            Destroy(oldParticleSystem);

        Instantiate(PlayerStats.Instance.CurrentWeapon.ParticleSystem, swingEffect);
    }

    /// <summary>
    /// Sends a request to stop the player from attacking.
    /// </summary>
    /// <param name="canHit">True sends a request to enable, false sends a request to disable.</param>
    public void ToggleHit(bool canHit)
    {
        _canPlayerAttack = canHit;
    }

    public void PowerUpSpawned(GameObject powerUpObject)
    {
        _spawnedPowerup = powerUpObject;
    }

    public void DestroySpawnedPowerUp()
    {
        if (_spawnedPowerup != null)
            Destroy(_spawnedPowerup);
    }

    /// <summary>
    /// Checks if the player can hit or not.
    /// </summary>
    public bool CanHit()
    {
        return _canPlayerAttack;
    }

    public void SetPowerUpCanSpawn(bool canSpawn)
    {
        CanSpawnPowerUps = canSpawn;
    }

    public void SetPowerUpSpawnPoints(Transform pointsParent)
    {
        GetComponent<PowerUpController>().SpawnPointsParent = pointsParent;
    }

    public Sprite ReturnPowerupSprite(PowerUpTypes powerup)
    {
        return _powerupSprites[(int)powerup];
    }
}
