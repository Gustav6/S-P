using UnityEngine;

[CreateAssetMenu(fileName = "StatReward", menuName = "ScriptableObjects/WaveRewards/Stat")]
public class StatReward : WaveReward
{
	[Tooltip("Modify a MAXIMUM of THREE properties. These are all noted in mathematical percentages meaning an increment in 0.1 corresponds to a 10% increase")]
	public StatBlock StatModifier = new(1, 1, 1, 1, 1, 1);
}
