using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
	[SerializeField] float _movementSpeed = 3;
	[SerializeField] ParticleSystem _bigPoofParticle;
	[SerializeField] Transform _feetPosition;

	internal Rigidbody2D rb;
	Animator _anim;

	internal bool isGrounded = true;

	public bool MovementLocked { get; private set; }

	float _particleDelayCounter = 0;

	private void Awake()
	{
		rb = GetComponent<Rigidbody2D>();
		_anim = transform.GetChild(0).GetComponent<Animator>();
	}

	private void Update()
	{
		if (MovementLocked)
			return;

		Vector2 input = new(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));

		_anim.SetBool("IsMoving", input.magnitude > 0);

		if (input.magnitude > 0)
        {
			_particleDelayCounter -= Time.deltaTime * Random.Range(0.85f, 1.15f);

			if (_particleDelayCounter <= 0)
			{
				ParticleManager.Instance.SpawnParticle(_bigPoofParticle, _feetPosition.position);
				_particleDelayCounter = 0.125f;
			}
		}

		if (isGrounded)
			rb.velocity = input.normalized * _movementSpeed * PlayerStats.Instance.GetStat(StatType.MovementSpeed);
	}

	public void ToggleMovementLock()
	{
		MovementLocked = !MovementLocked;
		rb.velocity = Vector2.zero;
	}
}