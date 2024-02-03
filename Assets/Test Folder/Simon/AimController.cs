using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AimController : MonoBehaviour
{
	[SerializeField] private float turnTime;

	[SerializeField] private Transform spriteTransform;
	[SerializeField] private Transform weaponSwingAnchor;
    [SerializeField] private Transform neckAnchor;

	private float _previousAimDirection = 1;

    private void Start()
    {
		_previousAimDirection = 1;
    }

    /// <summary>
    /// Aims the weapon of the entity towards a certain target.
    /// </summary>
    public void FaceTarget(Vector2 targetPosition)
    {
		Vector2 direction = (targetPosition - (Vector2)weaponSwingAnchor.position).normalized;
		float x = direction.x;
		direction.x *= weaponSwingAnchor.lossyScale.x;

		float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
		// Add 90 to align with mouse, because of how equation circle works.
		weaponSwingAnchor.localRotation = Quaternion.AngleAxis(angle + 90, Vector3.forward);

        // The minus is there because of how the rotation reacts when using Atan2.
        // This may cause issue for enemy rotation, if so: just remove it and debug the player.
        float currentAimDirection = -Mathf.Sign(x);

	 	RotateHead(weaponSwingAnchor.localRotation.eulerAngles.z);

		if (currentAimDirection != _previousAimDirection)
        {
			StopAllCoroutines();

			_previousAimDirection = currentAimDirection;
			StartCoroutine(TurnAround(currentAimDirection));
        }
	}

	private void RotateHead(float angle)
	{
		const float a = 180 / 3;
		float newZ;

		// Feels like these two if statements should swap what newZ is set to, but no
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
