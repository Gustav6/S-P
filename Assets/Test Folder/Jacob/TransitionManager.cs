using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TransitionManager : MonoBehaviour
{
    #region Singleton

    public static TransitionManager Instance;

	private void Awake()
	{
		if (Instance == null)
		{
			Instance = this;
			DontDestroyOnLoad(gameObject);
		}
		else
		{
			Debug.Log("More than one instance of TransitionManager found on " + gameObject.name + ". Destroying Instance");
			Destroy(this);
		}

		_anim = GetComponentInChildren<Animator>();
		_playerMovement = _playerTransform.GetComponent<PlayerMovement>();
	}

	#endregion

	[SerializeField] Transform _armTransform;
	[SerializeField] Transform _playerTransform;

	PlayerMovement _playerMovement;

	Animator _anim;

    private void Start()
    {
		Invoke(nameof(ApproachPlayer), 4);
    }

    void ApproachPlayer()
    {
		_armTransform.position = _playerTransform.position;
		_anim.Play("ArmApproach");
		Invoke(nameof(PickUpPlayer), 0.6f);
    }

	void PickUpPlayer()
    {
		_playerTransform.SetParent(_armTransform.GetChild(0));
		_playerMovement.ToggleMovementLock();
		_anim.Play("ArmPickUp");
		Invoke(nameof(DropPlayer), 2f);
	}

	void DropPlayer()
    {
		_anim.Play("ArmDrop");
		Invoke(nameof(UnParentPlayer), 0.42f);
    }

	void UnParentPlayer()
	{
		_playerMovement.ToggleMovementLock();
		_playerTransform.parent = null;
    }
}
