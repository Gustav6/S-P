using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Toggle : UI
{
    private ToggleStateManager ToggleStateManager { get; set; }

    [Header("Toggle variables")]
    public ToggleType toggleType;

    public bool toggleOn;
    [Range(0.1f, 1)] public float transitionTime = 0.3f;

    public override void Start()
    {
        ToggleStateManager = GetComponent<ToggleStateManager>();

        LoadFunction(ToggleStateManager);

        base.Start();
    }

    public override void Update()
    {
        UpdateFunction(ToggleStateManager);

        base.Update();
    }

    public void SaveToDataManager(UIDataManager manager, bool value, ToggleType type)
    {
        if (manager.toggleValues.ContainsKey(type))
        {
            manager.toggleValues[type] = value;

            for (int i = 0; i < UIDataManager.instance.Currentdata.toggleTypes.Length; i++)
            {
                if (type == (ToggleType)i)
                {
                    UIDataManager.instance.Currentdata.toggleValues[i] = value;
                }
            }
        }

        SaveSystem.Instance.SaveData(UIDataManager.instance.Currentdata);
    }
}
