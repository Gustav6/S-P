using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Toggle : UI
{
    private ToggleStateManager ToggleStateManager { get; set; }

    public ToggleType toggleType;

    public Color onColor = new(0.8f, 0, 0, 1);
    public Color offColor = new(0, 0.8f, 0, 1);

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
