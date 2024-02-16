using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HastePowerup : PowerUp
{
    bool _isUsingPowerup = false;

    private void Start()
    {
        powerUpSprite = EquipmentManager.Instance.ReturnPowerupSprite(PowerUpTypes.Haste);

        EquipmentManager.Instance.OnPowerUpEquipped?.Invoke();
    }

    public override void UsePowerUp()
    {
        EquipmentManager.Instance.OnPowerUpUsed?.Invoke();

        if (_isUsingPowerup)
            return;

        _isUsingPowerup = true;
        PlayerStats.Instance.ActivateAbilityStats(new StatBlock(1.25f, 1, 1, 1, 1, 1.25f));
        Invoke(nameof(OnDeactivatePowerUp), 10f);
    }

    public override void OnDeactivatePowerUp()
    {
        PlayerStats.Instance.DeActivateAbilityStats();
        PlayerStats.Instance.ClearEquippedAbility();
    }
}
