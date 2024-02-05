using UnityEngine;

public class WaveReward : ScriptableObject
{
	public string RewardName;
	public string RewardDescription; // Perhaps, might clutter the UI
	public int WaveUnlocked;
	public Sprite RewardSprite { get; private set; }
}

[CreateAssetMenu(fileName = "WeaponReward", menuName = "ScriptableObjects/WaveRewards/Weapon")]
public class WeaponReward : WaveReward
{
	WeaponSO containedWeapon;
}

[CreateAssetMenu(fileName = "StatReward", menuName = "ScriptableObjects/WaveRewards/Stat")]
public class StatReward : WaveReward
{
	[Tooltip("Modify a MAXIMUM of three properties. These are all noted in mathematical percentages meaning an increment in 0.1 corresponds to a 10% increase")]
	public PlayerStats StatModifier = new PlayerStats(1, 1, 1, 1, 1, 1, 1);
}
