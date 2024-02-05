using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour, IDamageable
{
	[SerializeField] float _movementSpeed = 3;
	[SerializeField] float _diStrength = 0.25f; // DI stands for direction input, used to reduce or enhance knockback when counteracting it with movement input.

	[SerializeField] float _turnTime;
	[SerializeField] Transform _spriteTransform;

	Rigidbody2D _rb;
	Animator _anim;

	bool _isImmune = false;
	bool _isGrounded = true;
	public bool MovementLocked { get; private set; }

	public float KnockbackPercent { get; set; }

	private void Awake()
	{
		_rb = GetComponent<Rigidbody2D>();
		_anim = transform.GetChild(0).GetComponent<Animator>();
	}

	private void Update()
	{
		if (Input.GetKeyDown(KeyCode.Space))
			TakeKnockback(Vector2.zero, 5, 0.25f);

		if (Input.GetKeyDown(KeyCode.E))
			ScreenShake.instance.Shake(0.5f, 0.25f, Vector2.zero);

		if (MovementLocked)
			return;

		Vector2 input = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));

		_anim.SetBool("IsMoving", input.magnitude > 0);

		if (_isGrounded)
			_rb.velocity = input.normalized * _movementSpeed;
	}

	/// <summary>
	/// Applies knockback to player and prevents them from moving for a specified time.
	/// </summary>
	/// <param name="sourcePosition">The position of the entity dealing the knockback.</param>
	/// <param name="knockbackMultiplier">Intensity of knockback.</param>
	/// <param name="stunDuration">Duration in seconds that movement is prevented after being hit.</param>
	public void TakeKnockback(Vector2 sourcePosition, float knockbackMultiplier, float stunDuration)
	{
		if (_isImmune)
			return;

		_isGrounded = false;
		_isImmune = true;

		Vector2 input = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical")).normalized;

		Vector2 knockbackVector = ((Vector2)transform.position - sourcePosition).normalized * knockbackMultiplier;

		Vector2 diVector = input * (knockbackVector.magnitude * _diStrength);

		_rb.AddForce(knockbackVector + diVector, ForceMode2D.Impulse);

		Invoke(nameof(ResetKB), stunDuration);
	}

	void ResetKB()
	{
		_isGrounded = true;
		_isImmune = false;
	}

	public void ToggleMovementLock()
	{
		MovementLocked = !MovementLocked;
		_rb.velocity = Vector2.zero;
	}
}
