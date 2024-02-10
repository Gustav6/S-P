using System.Threading.Tasks;
using UnityEngine;

public class CameraController : MonoBehaviour
{
	#region Singleton
	public static CameraController instance;

	private void Awake()
	{
		if (instance == null)
		{
			instance = this;
			_cameraTransform = transform.GetChild(0);
			_startPos = transform.position;
		}
		else
		{
			Debug.LogError("More than one instance of CameraController found on " + gameObject + ", destroying instance");
			Destroy(this);
		}
	}

	#endregion

	[SerializeField] Transform _targetTransform;
	[SerializeField] float _lookAheadDistance = 1.25f;
	bool _lookAheadEnabled = true;

	// Temporary curve, will add support for custom curves depending on the shake-source.
	[SerializeField] AnimationCurve _curve;

	Transform _cameraTransform;

	Vector2 _startPos;

    private void Update()
    {
		if (Input.GetKeyDown(KeyCode.Backspace))
		{
			Shake(0.5f, 0.25f);
		}

        if (!_lookAheadEnabled)
        {
			_cameraTransform.localPosition = Vector2.Lerp(_cameraTransform.localPosition, _startPos, Time.deltaTime * 3);
			return;
        }

		Vector2 direction = (Vector2)_targetTransform.position - _startPos;

		_cameraTransform.localPosition = Vector2.Lerp(_cameraTransform.localPosition, direction.normalized * _lookAheadDistance * (direction.magnitude / 6.5f), Time.deltaTime * 3);
	}

    #region Shake overload methods

    /// <summary>
    /// Shakes the camera around a point. Recommended duration of 0.5f and intensity of 0.25f
    /// </summary>
    /// <param name="duration">The duration in seconds of the shake effect</param>
    /// <param name="intensity">The strength of the shake</param>
    /// <param name="centerPoint">The object which the screen should shake around, useful for moving targets</param>
    public async void Shake(float duration, float intensity, Transform centerPoint)
	{
		float elapsedTime = 0f;

		while (elapsedTime < duration)
		{
			elapsedTime += Time.deltaTime;
			float strength = _curve.Evaluate(elapsedTime / duration) * intensity;
			transform.position = new Vector3(centerPoint.position.x, centerPoint.position.y, -10) + Random.insideUnitSphere * strength;
			await Task.Yield();
		}

		transform.position = new Vector3(centerPoint.position.x, centerPoint.position.y, -10);
	}

	/// <summary>
	/// Shakes the camera around a point. Recommended duration of 0.5f and intensity of 0.25f
	/// </summary>
	/// <param name="duration">The duration in seconds of the shake effect</param>
	/// <param name="intensity">The strength of the shake</param>
	/// <param name="centerPoint">The point which the screen should shake around, best for stationary targets</param>
	public async void Shake(float duration, float intensity, Vector2 centerPoint)
	{
		float elapsedTime = 0f;

		while (elapsedTime < duration)
		{
			elapsedTime += Time.deltaTime;
			float strength = _curve.Evaluate(elapsedTime / duration) * intensity;
			transform.position = new Vector3(centerPoint.x, centerPoint.y, -10) + Random.insideUnitSphere * strength;
			await Task.Yield();
		}

		transform.position = new Vector3(centerPoint.x, centerPoint.y, -10);
	}

	/// <summary>
	/// Shakes the camera around no specific point, useful paired with external camera movement such as lookahead. Recommended duration of 0.5f and intensity of 0.25f
	/// </summary>
	/// <param name="duration">The duration in seconds of the shake effect</param>
	/// <param name="intensity">The strength of the shake</param>
	public async void Shake(float duration, float intensity)
	{
		float elapsedTime = 0f;

		while (elapsedTime < duration)
		{
			elapsedTime += Time.deltaTime;
			float strength = _curve.Evaluate(elapsedTime / duration) * intensity;
			transform.position = new Vector3(_startPos.x, _startPos.y, -10) + Random.insideUnitSphere * strength;
			await Task.Yield();
		}

		transform.position = new Vector3(_startPos.x, _startPos.y, -10);
	}

	#endregion

	public void ToggleLookAhead()
    {
		_lookAheadEnabled = !_lookAheadEnabled;
    }
}