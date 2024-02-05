using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public struct PlayerStats
{
    public float AttackSpeed;
	public float DamageModifier;
	public float DamageResistanceModifier;
	public float KnockbackModifier;
	public float KnockbackResitanceModifier;
	public float MovementSpeedModifier;
	public float AbilityCooldownModifier;

	public PlayerStats(float attackSpeed, float damageModifier, float damageResistanceModifier, float knockbackModifier, float knockbackResitanceModifier, float movementSpeedModifier, float abilityCooldownModifier)
	{
		AttackSpeed = attackSpeed;
		DamageModifier = damageModifier;
		DamageResistanceModifier = damageResistanceModifier;
		KnockbackModifier = knockbackModifier;
		KnockbackResitanceModifier = knockbackResitanceModifier;
		MovementSpeedModifier = movementSpeedModifier;
		AbilityCooldownModifier = abilityCooldownModifier;
	}

	public static PlayerStats operator*(PlayerStats l, PlayerStats r)
	{
		return new PlayerStats(l.AttackSpeed * r.AttackSpeed,
			l.DamageModifier * r.DamageModifier,
			l.DamageResistanceModifier * r.DamageResistanceModifier,
			l.KnockbackModifier * r.KnockbackModifier,
			l.KnockbackResitanceModifier * r.KnockbackResitanceModifier,
			l.MovementSpeedModifier * r.MovementSpeedModifier,
			l.AbilityCooldownModifier * r.AbilityCooldownModifier);
	}

	public KeyValuePair<string, float> GetValue(int index)
    {
        switch (index)
        {
			case 0:
                return new KeyValuePair<string, float>("Attack Speed", AttackSpeed);

			case 1:
				return new KeyValuePair<string, float>("Damage Dealt", DamageModifier);

			case 2:
				return new KeyValuePair<string, float>("Damage Resitance ", DamageResistanceModifier);

			case 3:
				return new KeyValuePair<string, float>("Knockback Dealt", KnockbackModifier);

			case 4:
				return new KeyValuePair<string, float>("Knockback Resistance", KnockbackResitanceModifier);

			case 5:
				return new KeyValuePair<string, float>("Movement Speed", MovementSpeedModifier);

			case 6:
				return new KeyValuePair<string, float>("Ability Cooldown", AbilityCooldownModifier);
		}

		return new KeyValuePair<string, float>("Give Up", 2);
	}
}
