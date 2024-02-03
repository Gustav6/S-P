using UnityEngine;

public class WaveReward : ScriptableObject
{
	[SerializeField] string _rewardName;
	[SerializeField] string _rewardDescription; // Perhaps, might clutter the UI
	[SerializeField] int _waveUnlocked;
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
	[SerializeField] PlayerStats _statModifier = new PlayerStats(1, 1, 1, 1, 1, 1, 1);
}
