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

    /// <summary>
    /// Aims the weapon of the entity towards a certain target.
    /// </summary>
    public void FaceTarget(Vector2 targetPosition)
    {
		Vector2 direction = (targetPosition - (Vector2)weaponSwingAnchor.position).normalized;
		float x = direction.x;
		direction.x *= weaponSwingAnchor.lossyScale.x;

		float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;

		// Minus is here because of how the sprites interact with their local scale and different pivot points.
        float currentAimDirection = -Mathf.Sign(x);

		RotateBody(x, angle);

		if (currentAimDirection != _previousAimDirection)
        {
			if (Mathf.Abs(x)! < turnThreshold)
				return;

			StopAllCoroutines();

			_previousAimDirection = currentAimDirection;
			StartCoroutine(TurnAround(currentAimDirection));
        }
	}

	private void RotateBody(float x, float angle)
	{
		// Add 90 to align with mouse, because of how equation circle works.
		if (Mathf.Abs(x) >= 0.4f)
			weaponSwingAnchor.localRotation = Quaternion.AngleAxis(angle + 90, Vector3.forward);

		if (Mathf.Abs(x) >= 0.9)
		{
			// Subtract by 180 to adjust the head position so the eyes face the mouse rather than the neck.
			neckAnchor.localRotation = Quaternion.AngleAxis(angle - 180, Vector3.forward);
		}
	}

	private IEnumerator TurnAround(float direction)
	{
		WeaponManager.Instance.ToggleHit(false);

		float time = 0;

		neckAnchor.localRotation = Quaternion.Euler(0, 0, 0);

		while (time <= turnTime && direction != 0)
		{
			spriteTransform.localScale = new Vector2(Mathf.Lerp(0.5f * -direction, 0.5f * direction, time / turnTime), 0.5f);
			time += Time.deltaTime;
			yield return null;
		}

		spriteTransform.localScale = new Vector3(0.5f * direction, 0.5f, 0.5f);

		WeaponManager.Instance.ToggleHit(true);
	}

    #region Static Methods
	/// <summary>
	/// Returns the angle needed for an object to face a certain target.
	/// </summary>
    public static float PointToRotation(Vector2 thisPosition, Vector2 targetPosition)
    {
		Vector2 direction = (targetPosition - thisPosition).normalized;
		float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;

		return angle + 90;
	}

	/// <summary>
	/// Returns the angle needed for an object to face the mouse.
	/// </summary>
	public static float PointToRotation(Vector2 thisPosition)
	{
		Vector2 targetPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);

		Vector2 direction = (targetPosition - thisPosition).normalized;
		float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;

		return angle + 90;
	}
    #endregion
}
