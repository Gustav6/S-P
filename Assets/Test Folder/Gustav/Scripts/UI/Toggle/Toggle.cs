using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class Toggle : UI
{
    private ToggleStateManager ToggleStateManager { get; set; }

    [Header("Functions for v2")]
    [SerializeField] private List<Functions> selectedFunctions = new();
    private Dictionary<Functions, System.Action> functionLookup;

    [Header("Toggle variables")]
    public ToggleType toggleType;
    public ToggleVersion version;

    [SerializeField] private AudioMixer audioMixer;

    private const string MIXER_SFX = "SFXVolume";

    private const string MIXER_Music = "MusicVolume";

    private const string MIXER_Master = "MasterVolume";

    public bool toggleOn;
    [Range(0.1f, 1)] public float transitionTime = 0.3f;

    private bool fullScreen = true;

    public override void Start()
    {
        functionLookup = new Dictionary<Functions, System.Action>()
        {
            { Functions.ToggleFullscreen, ToggleFullscreen },
        };

        ToggleStateManager = GetComponent<ToggleStateManager>();

        LoadFunction(ToggleStateManager);

        base.Start();
    }

    public override void Update()
    {
        UpdateFunction(ToggleStateManager);

        base.Update();
    }

    public void ActivateSelectedFunctions()
    {
        for (int i = 0; i < selectedFunctions.Count; i++)
        {
            functionLookup[selectedFunctions[i]]?.Invoke();
        }
    }

    public void SaveToDataManager(UIDataManager manager, bool value, ToggleType type)
    {
        if (manager.toggleValues.ContainsKey(type))
        {
            manager.toggleValues[type] = value;

            for (int i = 0; i < UIDataManager.instance.CurrentData.toggleTypes.Length; i++)
            {
                if (type == (ToggleType)i)
                {
                    UIDataManager.instance.CurrentData.toggleValues[i] = value;
                }
            }
        }

        switch (type)
        {
            case ToggleType.MainOnOrOff:
                if (value)
                {
                    SetMasterVolume(UIDataManager.instance.sliderValues[SliderType.MainVolume]);
                }
                else
                {
                    SetMasterVolume(-80);
                }
                break;
            case ToggleType.MusicOnOrOff:
                if (value)
                {
                    SetMusicVolume(UIDataManager.instance.sliderValues[SliderType.MusicVolume]);
                }
                else
                {
                    SetMusicVolume(-80);
                }
                break;
            case ToggleType.SfxOnOrOff:
                if (value)
                {
                    SetSFXVolume(UIDataManager.instance.sliderValues[SliderType.SfxVolume]);
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

    private void ToggleFullscreen()
    {
        fullScreen = !fullScreen;
        Screen.fullScreen = fullScreen;
    }

    private enum Functions
    {
        ToggleFullscreen
    }
}

public enum ToggleVersion
{
    Version1,
    Version2,
}
