using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealPowerup : PowerUp
{
	bool _isUsingPowerup = false;

    private void Start()
    {
		powerUpSprite = EquipmentManager.Instance.ReturnPowerupSprite(PowerUpTypes.Heal);
    }

    public override void OnDeactivatePowerUp()
	{
		PlayerStats.Instance.DeActivateAbilityStats();
		PlayerStats.Instance.ClearEquippedAbility();
	}

	public override void UsePowerUp()
	{
		if (_isUsingPowerup)
			return;

		_isUsingPowerup = true;
		PlayerStats.Instance.HealDamage(75);
		PlayerStats.Instance.ActivateAbilityStats(new StatBlock(1, 1, 1.25f, 1, 1, 1));
		Invoke(nameof(OnDeactivatePowerUp), 10f);
	}
}
