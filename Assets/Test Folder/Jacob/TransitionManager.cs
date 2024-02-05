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
		_playerMovement.ToggleMovementLock();
		Invoke(nameof(PickUpPlayer), 0.85f);
    }

	public void PickUpPlayer()
    {
		_playerTransform.SetParent(_armTransform.GetChild(0));
		_playerMovement.GetComponent<Rigidbody2D>().velocity = Vector2.zero;
		_anim.Play("ArmPickUp");
		Invoke(nameof(DropPlayer), 2f);
	}

	public void DropPlayer()
    {
		_anim.Play("ArmDrop");
		Invoke(nameof(UnParentPlayer), 0.42f);
    }

	public void UnParentPlayer()
	{
		_playerMovement.ToggleMovementLock();
		_playerTransform.parent = null;
    }
}
