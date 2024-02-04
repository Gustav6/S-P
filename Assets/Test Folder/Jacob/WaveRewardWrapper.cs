using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaveRewardWrapper : Interactable
{
	[SerializeField] WaveReward containedReward;

	[SerializeField] GameObject statRewardPanel, weaponRewardPanel;

	private void Start()
	{
		Instantiate(statRewardPanel, transform);
	}

	public override void EnterInteractionRange()
	{
		throw new System.NotImplementedException();
	}

	public override void ExitInteractionRange()
	{
		throw new System.NotImplementedException();
	}

	public override void Interact()
	{
		throw new System.NotImplementedException();
	}

	private void OnDrawGizmosSelected()
	{
		Gizmos.DrawWireSphere(transform.position, InteractionRadius);
	}
}
