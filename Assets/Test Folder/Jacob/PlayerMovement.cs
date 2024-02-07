using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
	[SerializeField] float _movementSpeed = 3;

	internal Rigidbody2D rb;
	Animator _anim;

	internal bool isGrounded = true;

	public bool MovementLocked { get; private set; }

	private void Awake()
	{
		rb = GetComponent<Rigidbody2D>();
		_anim = transform.GetChild(0).GetComponent<Animator>();
	}

	private void Update()
	{
		if (MovementLocked)
			return;

		Vector2 input = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));

		_anim.SetBool("IsMoving", input.magnitude > 0);

		if (isGrounded)
			rb.velocity = input.normalized * _movementSpeed;
	}

	public void ToggleMovementLock()
	{
		MovementLocked = !MovementLocked;
		rb.velocity = Vector2.zero;
	}
}

