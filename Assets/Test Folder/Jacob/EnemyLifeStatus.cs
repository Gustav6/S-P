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
	}

	private void OnMouseDown()
	{
		Destroy(gameObject);
	}
}
