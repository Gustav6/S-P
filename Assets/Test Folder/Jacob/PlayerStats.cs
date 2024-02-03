using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public struct PlayerStats
{
    [SerializeField] float AttackSpeed;
	[SerializeField] float DamageModifier;
	[SerializeField] float DamageResistanceModifier;
	[SerializeField] float KnockbackModifier;
	[SerializeField] float KnockbackResitanceModifier;
	[SerializeField] float MovementSpeedModifier;
	[SerializeField] float AbilityCooldownModifier;

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
}
