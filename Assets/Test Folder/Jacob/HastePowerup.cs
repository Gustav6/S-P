using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HastePowerup : PowerUp
{
    bool _isUsingPowerup = false;

    public override void UsePowerUp()
    {
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
