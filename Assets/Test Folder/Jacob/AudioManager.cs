using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine.Audio;
using UnityEngine;
using UnityEngine.SceneManagement;

public class AudioManager : MonoBehaviour
{
    // Singleton
    public static AudioManager Instance;

    [SerializeField] AudioMixerGroup audioMixer;

    [SerializeField] Sound[] sounds;

    void Awake()
    {
        #region Singleton

        if (Instance == null)
            Instance = this;
        else
        {
            Debug.LogWarning("More than one instance of AudioManager found on " + gameObject + "!! Destroying object");
            Destroy(gameObject);
        }

        #endregion

        foreach (Sound s in sounds)
        {
            s.Source = gameObject.AddComponent<AudioSource>();
            s.Source.outputAudioMixerGroup = audioMixer;
            s.Source.clip = s.Clip;
            s.Source.volume = s.Volume;
            s.Source.pitch = s.Pitch;
            s.Source.loop = s.Loop;
        }
    }

    public void Play(string name)
    {
        Sound s = Array.Find(sounds, sound => sound.Name == name);

        if (s == null)
        {
            Debug.LogWarning("Sound: " + name + " not found!");
            return;
        }

        if (s.RandomizePitch > 0)
            s.Source.pitch = UnityEngine.Random.Range(s.Pitch - s.RandomizePitch, s.Pitch + s.RandomizePitch);

        if (!s.Source.isPlaying)
            s.Source.Play();
    }

    public void Stop(string name)
    {
        Sound s = Array.Find(sounds, sound => sound.Name == name);

        if (s == null)
        {
            Debug.LogWarning("Sound: " + name + " not found!");
            return;
        }

        s.Source.Stop();
    }
}


[Serializable]
public class Sound
{
	public string Name;

	public AudioClip Clip;

	[Range(0f, 2f)]
	public float Volume;
	[Range(0.1f, 3f)]
	public float Pitch;
    public float RandomizePitch = 0;

	public bool Loop;

	[HideInInspector]
	public AudioSource Source;
}

