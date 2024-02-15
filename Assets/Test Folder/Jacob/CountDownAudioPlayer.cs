using UnityEngine;

public class CountDownAudioPlayer : MonoBehaviour
{
    public void PlaySFX(string sound)
	{
		AudioManager.Instance.PlaySound(sound);
	}
}
