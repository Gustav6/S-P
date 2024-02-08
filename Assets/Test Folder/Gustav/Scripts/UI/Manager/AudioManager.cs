using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class AudioManager : MonoBehaviour
{
    [SerializeField] private AudioClip[] clips;
    private AudioSource audioSource;


    void Start()
    {
        audioSource = GetComponent<AudioSource>();
    }

    public void PlaySound(AudioType audio)
    {
        float volume = UIManager.UIDataManagerInstance.sliderValues[SliderType.MainVolume];
        audioSource.PlayOneShot(clips[(int)audio], volume);
    }
}

public enum AudioType
{
    ClickSound,
    SelectSound,
}
