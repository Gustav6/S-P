using UnityEngine;

public class WaveReward : ScriptableObject
{
	public string RewardName;
	public int WaveUnlocked;
}

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

[CreateAssetMenu(fileName = "StatReward", menuName = "ScriptableObjects/WaveRewards/Stat")]
public class StatReward : WaveReward
{
	[Tooltip("Modify a MAXIMUM of THREE properties. These are all noted in mathematical percentages meaning an increment in 0.1 corresponds to a 10% increase")]
	public StatBlock StatModifier = new(1, 1, 1, 1, 1, 1, 1);
}