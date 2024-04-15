using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class Slider : UI
{
    private SliderStateManager SliderStateManager { get; set; }

    [Header("Slider variables")]
    public SliderType sliderType;

    [SerializeField] private AudioMixer audioMixer;

    private const string MIXER_SFX = "SFXVolume";

    private const string MIXER_Music = "MusicVolume";

    private const string MIXER_Master = "MasterVolume";

    public override void Start()
    {
        SliderStateManager = GetComponent<SliderStateManager>();

        LoadFunction(SliderStateManager);

        base.Start();
    }

    public override void Update()
    {
        UpdateFunction(SliderStateManager);

        base.Update();
    }
    public void SaveToDataManager(UIDataManager manager, float value, SliderType type)
    {
        if (manager.sliderValues.ContainsKey(type))
        {
            manager.sliderValues[type] = value;

            for (int i = 0; i < UIDataManager.instance.CurrentData.sliderTypes.Length; i++)
            {
                if (type == (SliderType)i)
                {
                    UIDataManager.instance.CurrentData.sliderValues[i] = value;
                }
            }
        }

        switch (type)
        {
            case SliderType.MainVolume:
                if (value > 0)
                {
                    SetMasterVolume(value);
                }
                else
                {
                    SetMasterVolume(-80);
                }
                break;
            case SliderType.MusicVolume:
                if (value > 0)
                {
                    SetMusicVolume(value);
                }
                else
                {
                    SetMusicVolume(-80);
                }
                break;
            case SliderType.SfxVolume:
                if (value > 0)
                {
                    SetSFXVolume(value);
                }
                else
                {
                    SetSFXVolume(-80);
                }
                break;
            default:
                break;
        }

        SaveSystem.Instance.SaveData(UIDataManager.instance.CurrentData);
    }

    private void SetMasterVolume(float value)
    {
        audioMixer.SetFloat(MIXER_Master, Mathf.Log10(value) * 20); 
    }

    private void SetMusicVolume(float value)
    {
        audioMixer.SetFloat(MIXER_Music, Mathf.Log10(value) * 20);
    }

    private void SetSFXVolume(float value)
    {
        audioMixer.SetFloat(MIXER_SFX, Mathf.Log10(value) * 20);
    }
}
