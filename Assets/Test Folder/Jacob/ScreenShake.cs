using System.Threading.Tasks;
using UnityEngine;

public class ScreenShake : MonoBehaviour
{
	#region Singleton
	public static ScreenShake instance;

	private void Awake()
	{
		if (instance == null)
		{
			instance = this;
			DontDestroyOnLoad(gameObject);
		}
		else
		{
			Destroy(this);
		}
	}

	#endregion

	// Temporary curve, will add support for custom curves depending on the shake-source.
	[SerializeField] AnimationCurve _curve;

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
			Camera.main.transform.position = new Vector3(centerPoint.position.x, centerPoint.position.y, -10) + UnityEngine.Random.insideUnitSphere * strength;
			await Task.Yield();
		}

		Camera.main.transform.position = new Vector3(centerPoint.position.x, centerPoint.position.y, -10);
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
			Camera.main.transform.position = new Vector3(centerPoint.x, centerPoint.y, -10) + UnityEngine.Random.insideUnitSphere * strength;
			await Task.Yield();
		}

		Camera.main.transform.position = new Vector3(centerPoint.x, centerPoint.y, -10);
	}

	#endregion
}