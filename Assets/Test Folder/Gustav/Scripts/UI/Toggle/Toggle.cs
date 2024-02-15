using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Toggle : UI
{
    private ToggleStateManager ToggleStateManager { get; set; }

    [Header("Functions for v2")]
    [SerializeField] private List<Functions> selectedFunctions = new();
    private Dictionary<Functions, System.Action> functionLookup;

    [Header("Toggle variables")]
    public ToggleType toggleType;
    public ToggleVersion version;

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

        SaveSystem.Instance.SaveData(UIDataManager.instance.CurrentData);
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
