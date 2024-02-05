using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class WaveRewardInteractable : Interactable
{
	[SerializeField] WaveManager _waveManager;

	[SerializeField] WaveReward _containedReward;
	
	GameObject _rewardPanel;

	[SerializeField] Color _redColor;

	private void Start()
	{

	}

	public void SetContainedReward(WaveReward newReward)
    {
		_containedReward = newReward;

		CreateRewardPanel(newReward is StatReward);
    }

	void CreateRewardPanel(bool isStatReward)
	{
		if (_rewardPanel != null)
			Destroy(_rewardPanel);

		if (isStatReward)
        {
            _rewardPanel = Instantiate(_waveManager.StatRewardPanel);
			SetStatPanelStats(_containedReward as StatReward);
        }
        else
		{
			_rewardPanel = Instantiate(_waveManager.WeaponRewardPanel, gameObject.transform);

		}
    }

	void SetStatPanelStats(StatReward currentReward)
	{
		Transform panel = _rewardPanel.transform.GetChild(0);

		int textIndex = 2;

        for (int i = 0; i < 7; i++)
        {
			float currentModifier = currentReward.StatModifier.GetValue(i).Value - 1;
			string signPrefix = currentModifier < 0 ? "-" : "+";

			if (currentModifier != 0)
			{
				if (textIndex > 4)
                {
					Debug.LogError("Too many values modified in " + currentReward + ", a maximum of three values are allowed to be modified");
					return;
				}

				TMP_Text text = panel.GetChild(textIndex).GetComponent<TMP_Text>();

				if (signPrefix == "-")
					text.color = _redColor;

				text.text = signPrefix + " " + Mathf.Abs((currentModifier * 100)).ToString() + 
																		  "\n" + currentReward.StatModifier.GetValue(i).Key.ToString();
				textIndex++;

			}
			else
            {
				panel.GetChild(textIndex).GetComponent<TMP_Text>().text = "";
			}
        }

		panel.GetChild(1).GetComponent<TMP_Text>().text = currentReward.RewardName;

	}

	void SetWeaponPanelStats(WeaponReward currentReward)
    {

    }

	public override void EnterInteractionRange()
	{
		_rewardPanel.SetActive(true);
	}

	public override void ExitInteractionRange()
	{
		_rewardPanel.SetActive(false);
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
