using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AimController : MonoBehaviour
{
	[SerializeField] private float turnTime;

	[SerializeField] private Transform spriteTransform;

	private Transform _weaponSwingAnchor;

	private float _previousAimDirection;

    private void Awake()
    {
		_weaponSwingAnchor = transform.GetChild(0).GetChild(0);

		_previousAimDirection = 1;
    }

    /// <summary>
    /// Aims the weapon of the entity towards a certain target.
    /// </summary>
    public void FaceTarget(Vector2 targetPosition)
    {
		Vector2 direction = (targetPosition - (Vector2)_weaponSwingAnchor.position).normalized;
		float x = direction.x;
		direction.x *= _weaponSwingAnchor.lossyScale.x;

		float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
		// Add 90 to align with mouse, because of how equation circle works.
		_weaponSwingAnchor.localRotation = Quaternion.AngleAxis(angle + 90, Vector3.forward);

		// The minus is there because of how the rotation reacts when using Atan2.
		// This may cause issue for enemy rotation, if so: just remove it and debug the player.
		float currentAimDirection = -Mathf.Sign(x);

		if (currentAimDirection != _previousAimDirection)
        {
			StopAllCoroutines();

			_previousAimDirection = currentAimDirection;
			StartCoroutine(TurnAround(currentAimDirection));
        }
	}

	private IEnumerator TurnAround(float direction)
	{
		float time = 0;

		while (time <= turnTime && direction != 0)
		{
			spriteTransform.localScale = new Vector2(Mathf.Lerp(-direction, direction, time / turnTime), 1);
			time += Time.deltaTime;
			yield return null;
		}

		spriteTransform.localScale = new Vector3(direction, 1, 1);
	}
}
