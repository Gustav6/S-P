using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using System.Runtime.CompilerServices;
using UnityEngine;

// Simon
public class AimController : MonoBehaviour
{
	[SerializeField] private float turnTime;

    [SerializeField] private Transform spriteTransform;
	[SerializeField] private Transform weaponSwingAnchor;
    [SerializeField] private Transform neckAnchor;

	private float turnThreshold = 0.45f;
	private float _topHeadRotation = -44, _bottomHeadRotation = 44;
	private float _topArmRotation = -165, _bottomArmRotation = -14;

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
		float distance = Vector2.Distance((Vector2)transform.position, targetPosition);

		// Minus is here because of how the sprites interact with their local scale and different pivot points.
        float currentAimDirection = -Mathf.Sign(x);

		if (distance > 0.45f)
			RotateBody(x, angle);

		if (currentAimDirection != _previousAimDirection && Mathf.Abs(x) >= turnThreshold)
        {
			StopAllCoroutines();

			_previousAimDirection = currentAimDirection;
			StartCoroutine(TurnAround(currentAimDirection));
        }
	}

	/// <summary>
	/// Aims the weapon of the entity in a certain direction.
	/// </summary>
	public void FaceDirection(Vector2 direction)
    {
		float x = direction.x;
		float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;

		// Minus is here because of how the sprites interact with their local scale and different pivot points.
		float currentAimDirection = -Mathf.Sign(x);

		RotateBody(x, angle);

		if (currentAimDirection != _previousAimDirection && Mathf.Abs(x) >= turnThreshold)
		{
			StopAllCoroutines();

			_previousAimDirection = currentAimDirection;
			StartCoroutine(TurnAround(currentAimDirection));
		}
	}

	/// <summary>
	/// Rotates the neck and the body of the entity, also handles turning of the body.
	/// </summary>
    private void RotateBody(float x, float angle)
	{
		void SetLocalRotation(bool isNeckRotation)
		{
			if (!isNeckRotation)
			{
                if (IsCloserToA(90, -90, angle))
                    weaponSwingAnchor.localRotation = Quaternion.Euler(Vector3.forward * _topArmRotation);
                else
                {
                    weaponSwingAnchor.localRotation = Quaternion.Euler(Vector3.forward * _bottomArmRotation);
                }
            }

            if (IsCloserToA(90, -90, angle))
                neckAnchor.localRotation = Quaternion.Euler(Vector3.forward * _topHeadRotation);
            else
            {
                neckAnchor.localRotation = Quaternion.Euler(Vector3.forward * _bottomHeadRotation);
            }
        }

		if (_previousAimDirection != -Mathf.Sign(x))
		{
			SetLocalRotation(true);
			SetLocalRotation(false);
            return;
        }

		// Add 90 to align with mouse, because of how equation circle works.
		if (Mathf.Abs(x) >= turnThreshold)
			weaponSwingAnchor.localRotation = Quaternion.AngleAxis(angle + 90, Vector3.forward);
		else
		{
			SetLocalRotation(false);
		}

		if (Mathf.Abs(x) >= 0.9)
		{
			// Subtract by 180 to adjust the head position so the eyes face the mouse rather than the neck.
			neckAnchor.localRotation = Quaternion.AngleAxis(angle - 180, Vector3.forward);
		}
		else
        {
			SetLocalRotation(true);
        }
    }

	private bool IsCloserToA(float a, float b, float c)
	{
        float distToA = Mathf.Abs(c - a);
        float distToB = Mathf.Abs(c - b);

        return distToA < distToB;
    }

	/// <summary>
	/// Turns the the whole entity in a certain direction.
	/// </summary>
    private IEnumerator TurnAround(float direction)
	{
		EquipmentManager.Instance.ToggleHit(false);

		float time = 0;

		while (time <= turnTime && direction != 0)
		{
			spriteTransform.localScale = new Vector2(Mathf.Lerp(0.5f * -direction, 0.5f * direction, time / turnTime), 0.5f);
			time += Time.deltaTime;
			yield return null;
		}

		spriteTransform.localScale = new Vector3(0.5f * direction, 0.5f, 0.5f);

		EquipmentManager.Instance.ToggleHit(true);
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
