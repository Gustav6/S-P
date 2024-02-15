using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyLifeStatus : MonoBehaviour
{
	public int Value;

	public delegate void OnDeath(int value);
	public OnDeath OnDeathCallback;

	private void OnDestroy()
	{
		OnDeathCallback.Invoke(Value);
		PlayerStats.Instance.AddScore(Value * 10);
		PopupManager.Instance.SpawnText("+" + (Value * 10).ToString(), transform.position, 1.75f);
	}

	private void OnMouseDown()
	{
		Destroy(gameObject);
	}
}