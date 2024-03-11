using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using static Transition;

public class Button : UI
{
    private ButtonStateManager ButtonStateManager { get; set; }

    [Header("Button variables")]
    [SerializeField] private List<Functions> selectedFunctions = new();
    private Dictionary<Functions, Action> functionLookup;

    [Range(0.1f, 5)] public float transitionDuration;

    public ActiveScene NewScene;
    public bool playTransition;

    [Header("Drag in the prefab here")]
    public GameObject prefabToEnable;
    public GameObject prefabToDisable;

    public override void Start()
    {
        functionLookup = new Dictionary<Functions, System.Action>()
        {
            { Functions.SwitchScene, SceneTransition },
            { Functions.DisablePrefab, DisablePrefab },
            { Functions.EnablePrefab, EnablePrefab },
            { Functions.QuitGame, Application.Quit },
            { Functions.UnPause, UnPause },
            { Functions.ClearLeaderboard, ClearLeaderboard }
        };

        ButtonStateManager = GetComponent<ButtonStateManager>();

        LoadFunction(ButtonStateManager);

        base.Start();
    }

    public override void Update()
    {
        UpdateFunction(ButtonStateManager);

        base.Update();
    }

    private void UnPause()
    {
        UIStateManager.Instance.PauseMenuActive = false;
    }

    private void EnablePrefab()
    {
        if (prefabToEnable != UIStateManager.Instance.ActivePrefab)
        {
            UIStateManager.Instance.PrefabToEnable = prefabToEnable;
        }
    }

    private void DisablePrefab()
    {
        if (prefabToEnable != UIStateManager.Instance.ActivePrefab)
        {
            UIStateManager.Instance.PrefabToDisable = prefabToDisable;
            UIStateManager.Instance.PlayTransition = playTransition;
        }
    }

    private void SceneTransition()
    {
        if (CoverManager.Instance != null && playTransition)
        {
            ExecuteOnCompletion execute = SwitchScene;

            CoverManager.Instance.Cover(transitionDuration, execute);
        }
        else if (CoverManager.Instance == null || !playTransition)
        {
            SwitchScene();
        }
    }

    private void SwitchScene()
    {
        SceneManager.LoadScene((int)NewScene);
    }

    private void ClearLeaderboard()
    {
        UIDataManager.instance.waveNames = new string[5];
        UIDataManager.instance.scoreNames = new string[5];
        UIDataManager.instance.wave = new int[5];
        UIDataManager.instance.score = new int[5];

        for (int i = 0; i < UIDataManager.instance.CurrentData.scoreLeadBoardNames.Length; i++)
        {
            UIDataManager.instance.CurrentData.scoreLeadBoardNames[i] = "";
            UIDataManager.instance.CurrentData.scoreLeaderBoardValues[i] = 0;
        }

        for (int i = 0; i < UIDataManager.instance.CurrentData.waveLeadBoardNames.Length; i++)
        {
            UIDataManager.instance.CurrentData.waveLeadBoardNames[i] = "";
            UIDataManager.instance.CurrentData.waveLeaderBoardValues[i] = 0;
        }

        SaveSystem.Instance.SaveData(UIDataManager.instance.CurrentData);
        UpdateLeadboard();
    }

    public void ActivateSelectedFunctions()
    {
        for (int i = 0; i < selectedFunctions.Count; i++)
        {
            functionLookup[selectedFunctions[i]]?.Invoke();
        }
    }

    private enum Functions
    {
        SwitchScene,
        DisablePrefab,
        EnablePrefab,
        QuitGame,
        UnPause,
        ClearLeaderboard
    }
}

public enum ActiveScene
{
    MainMenu,
    Game,
}
