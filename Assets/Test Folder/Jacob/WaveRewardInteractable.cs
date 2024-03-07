using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;
using TMPro;

public class WaveRewardInteractable : Interactable
{
	[SerializeField] GameObject _statRewardPanel, _weaponRewardPanel;

	[SerializeField] WaveReward _containedReward;
	
	GameObject _rewardPanel;

	[SerializeField] Color _redColor;

	Animator _panelAnimator;
	
    public delegate void OnRewardClaimed();
	public OnRewardClaimed OnRewardClaimedCallback;

	StatBlock _randomStatBlock;

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
            _rewardPanel = Instantiate(_statRewardPanel, gameObject.transform);
			SetStatPanelStats(_containedReward as StatReward);
        }
        else
		{
			_rewardPanel = Instantiate(_weaponRewardPanel, gameObject.transform);
			SetWeaponPanelStats(_containedReward as WeaponReward);
		}

		_panelAnimator = _rewardPanel.GetComponent<Animator>();
    }

	void SetStatPanelStats(StatReward currentReward)
	{
		Transform panel = _rewardPanel.transform.GetChild(0);

		int textIndex = 2;

        for (int i = 0; i < 6; i++)
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

				text.text = signPrefix + " " + Mathf.Abs(Mathf.RoundToInt(currentModifier * 100)).ToString() + "%\n" + currentReward.StatModifier.GetValue(i).Key.ToString();
				textIndex++;
			}
			else if (textIndex <= 4)
            {
				panel.GetChild(textIndex).GetComponent<TMP_Text>().text = "";
			}
        }

		panel.GetChild(1).GetComponent<TMP_Text>().text = currentReward.RewardName;
	}

	void SetWeaponPanelStats(WeaponReward currentReward)
    {
		Transform panel = _rewardPanel.transform.GetChild(0);

		panel.GetChild(1).GetComponent<TMP_Text>().text = currentReward.RewardName;

		panel.GetChild(2).GetComponent<Image>().sprite = currentReward.RewardSprite;

		int randomStatIndex = Random.Range(0, 6);

		float randomStatValue = 1;

        switch (Random.Range(0, 3))
        {
			case 0:
				randomStatValue = 1.1f;
				break;

			case 1:
				randomStatValue = 0.9f;
				break;

			default:
				break;
        }

		_randomStatBlock = new(randomStatIndex, randomStatValue);

		float currentModifier = randomStatValue - 1;
		string signPrefix = currentModifier < 0 ? "-" : "+";

		if (currentModifier != 0)
		{
			TMP_Text text = panel.GetChild(3).GetComponent<TMP_Text>();

			if (signPrefix == "-")
				text.color = _redColor;

			text.text = signPrefix + " " + Mathf.Abs(currentModifier * 100).ToString() + "%\n" + currentReward.StatModifier.GetValue(randomStatIndex).Key.ToString();
		}
		else
		{
			panel.GetChild(3).GetComponent<TMP_Text>().text = "";
		}

	}

	public override void EnterInteractionRange()
	{
		_panelAnimator.Play("PanelExpand");
	}

	public override void ExitInteractionRange()
	{
		_panelAnimator.Play("PanelClose");
	}

	public override void Interact()
	{
		if (_containedReward is StatReward)
        {
			if (!TutorialManager.Instance.hasAttacked)
            {
				PopupManager.Instance.SpawnText("You must finish the previous part of the tutorial first", transform.position, 3f);
				return;
            }

			PopupManager.Instance.SpawnText("Stat boost added!\n" + _containedReward.RewardName, (Vector2)transform.position + new Vector2(0, 1), 2);
            PlayerStats.Instance.AddStatModifier((_containedReward as StatReward).StatModifier);
        }
        else
        {
			PopupManager.Instance.SpawnText("Weapon aquired!\n" + _containedReward.RewardName, (Vector2)transform.position + new Vector2(0, 1), 2);
			EquipmentManager.Instance.SwitchWeapon((_containedReward as WeaponReward).ContainedWeapon);
			PlayerStats.Instance.NewWeaponEquipped(_randomStatBlock);
		}

		OnRewardClaimedCallback.Invoke();
	}

	private void OnDrawGizmosSelected()
	{
		Gizmos.DrawWireSphere(transform.position, InteractionRadius);
	}
}
