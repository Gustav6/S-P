using UnityEngine;

[CreateAssetMenu(fileName = "WeaponReward", menuName = "ScriptableObjects/WaveRewards/Weapon")]
public class WeaponReward : WaveReward
{
	public WeaponSO ContainedWeapon;
	public Sprite RewardSprite;

	public StatBlock StatModifier { get; private set; }

	public void SetStatModifier(StatBlock newStatModifier)
	{
		StatModifier = newStatModifier;
	}
}
