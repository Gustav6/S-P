using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AimController : MonoBehaviour
{
	[SerializeField] private float turnTime;
    [SerializeField] private float turnThreshold = 0.45f;

    [SerializeField] private Transform spriteTransform;
	[SerializeField] private Transform weaponSwingAnchor;
    [SerializeField] private Transform neckAnchor;

	private float _previousAimDirection = 1;
	private float _angle;

	private bool _isArmReseting;

	// TODO: Add an IEnumarator so the arm of the entity moves towards target slowly after an attack instead of snapping.

    /// <summary>
    /// Aims the weapon of the entity towards a certain target.
    /// </summary>
    public void FaceTarget(Vector2 targetPosition)
    {
		Vector2 direction = (targetPosition - (Vector2)weaponSwingAnchor.position).normalized;
		float x = direction.x;
		direction.x *= weaponSwingAnchor.lossyScale.x;

		float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
		_angle = angle;

		if (_isArmReseting)
			return;

		// Add 90 to align with mouse, because of how equation circle works.
		weaponSwingAnchor.localRotation = Quaternion.AngleAxis(angle + 90, Vector3.forward);

		// Minus is here because of how the sprites interact with their local scale and different pivot points.
        float currentAimDirection = -Mathf.Sign(x);

        RotateHead(weaponSwingAnchor.localRotation.eulerAngles.z);

		if (currentAimDirection != _previousAimDirection)
        {
			if (Mathf.Abs(x)! < turnThreshold)
				return;

			StopAllCoroutines();

			_previousAimDirection = currentAimDirection;
			StartCoroutine(TurnAround(currentAimDirection));
        }
	}

	public void StartFacingTargetAfterAttack()
	{
		StartCoroutine(FaceTargetAfterAttack());
	}

	// TODO: Make work
	private IEnumerator FaceTargetAfterAttack()
	{
		_isArmReseting = true;

        float time = 0;
		float startAngle = Mathf.Atan2(weaponSwingAnchor.position.y, weaponSwingAnchor.position.x) * Mathf.Rad2Deg;
		float b;

		Debug.Log($"Start: {startAngle}\nTarget: {_angle}");

        while (time < 1f)
		{
			b = Mathf.Lerp(startAngle, _angle, time / 1);
            weaponSwingAnchor.localRotation = Quaternion.AngleAxis(b + 90, Vector3.forward);

            time += Time.deltaTime;
            yield return null;
		}

		_isArmReseting = false;
	}

	private void RotateHead(float angle)
	{
		const float a = 180 / 3;
		float newZ;

		// Feels like these two if statements should swap what newZ is set to, but no.
		if (angle >= 360 - a)
			newZ = 30;
		else if (angle <= 180 + a)
			newZ = -20;
		else
		{
			newZ = 0;
		}

		neckAnchor.localRotation = Quaternion.Euler(0, 0, newZ);
	}

	private IEnumerator TurnAround(float direction)
	{
		float time = 0;

		while (time <= turnTime && direction != 0)
		{
			spriteTransform.localScale = new Vector2(Mathf.Lerp(0.5f * -direction, 0.5f * direction, time / turnTime), 0.5f);
			time += Time.deltaTime;
			yield return null;
		}

		spriteTransform.localScale = new Vector3(0.5f * direction, 0.5f, 0.5f);
	}
}
