using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public struct StatBlock
{
	public float AttackSpeed;
	public float DamageModifier;
	public float DamageResistanceModifier;
	public float KnockbackModifier;
	public float KnockbackResitanceModifier;
	public float MovementSpeedModifier;

	public StatBlock(float attackSpeed, float damageModifier, float damageResistanceModifier, float knockbackModifier, float knockbackResitanceModifier, float movementSpeedModifier)
	{
		AttackSpeed = attackSpeed;
		DamageModifier = damageModifier;
		DamageResistanceModifier = damageResistanceModifier;
		KnockbackModifier = knockbackModifier;
		KnockbackResitanceModifier = knockbackResitanceModifier;
		MovementSpeedModifier = movementSpeedModifier;
	}

	public StatBlock(int index, float value)
    {
        AttackSpeed = 1;
        DamageModifier = 1;
        DamageResistanceModifier = 1;
        KnockbackModifier = 1;
        KnockbackResitanceModifier = 1;
        MovementSpeedModifier = 1;

        switch (index)
        {
            case 0:
                AttackSpeed = value;
                break;

            case 1:
                DamageModifier = value;
                break;

            case 2:
                DamageResistanceModifier = value;
                break;

            case 3:
                KnockbackModifier = value;
                break;

            case 4:
                KnockbackResitanceModifier = value;
                break;

            case 5:
                MovementSpeedModifier = value;
                break;

            default:
                break;
        }
    }

    public static StatBlock operator*(StatBlock l, StatBlock r)
	{
        return new StatBlock(l.AttackSpeed * r.AttackSpeed,
            l.DamageModifier * r.DamageModifier,
            l.DamageResistanceModifier * r.DamageResistanceModifier,
            l.KnockbackModifier * r.KnockbackModifier,
            l.KnockbackResitanceModifier * r.KnockbackResitanceModifier,
            l.MovementSpeedModifier * r.MovementSpeedModifier);
	}

	/// <summary>
	/// Cast statType from the StatType enum to an integer
	/// </summary>
	/// <param name="statType">Cast statType from the StatType enum to an integer</param>
	/// <returns></returns>
	public KeyValuePair<string, float> GetValue(int statType)
    {
        switch (statType)
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

            default:
                break;
        }

        return new KeyValuePair<string, float>("Give Up", 2);
    }
}

public enum StatType
{
	AttackSpeed,
	DamageDealt,
	DamageResistance,
	KnockbackDealt,
	KnockbackResistance,
	MovementSpeed
}