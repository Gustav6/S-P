using UnityEngine;

public class CountDownAudioPlayer : MonoBehaviour
{
    public void PlaySFX(string sound)
	{
		AudioManager.Instance.Play(sound);
	}
}
