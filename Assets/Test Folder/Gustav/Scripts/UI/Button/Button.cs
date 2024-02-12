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

    [Range(0.1f, 5)] public float transitionDuration;

    public bool transitionToScene;
    public NewScene NewScene;

    public bool transitionToPrefab;
    public bool scaleTransition;
    public bool moveTransition;

    [Header("Drag in the prefab here")]
    public GameObject prefabToSpawn;

    public override void Start()
    {
        ButtonStateManager = GetComponent<ButtonStateManager>();

        LoadFunction(ButtonStateManager);

        base.Start();
    }

    public override void Update()
    {
        UpdateFunction(ButtonStateManager);

        base.Update();
    }

    public void StartSceneTransition()
    {
        ExecuteOnCompletion execute = null;

        StartPrefabTransition();

        execute += SwitchScene;

        PanelManager.FadeOut(transitionDuration, new Color(0, 0, 0, 1), execute);
        Debug.Log("Change Scene");
    }

    public void StartPrefabTransition()
    {
        UIManager.instance.prefabToSpawn = prefabToSpawn;
        UIManager.instance.MoveUIThenRemove(transitionDuration, null);
        Debug.Log("Move In Prefab");
    }

    private void SwitchScene()
    {
        SceneManager.LoadScene((int)NewScene);
    }
}

public enum NewScene
{
    Game,
    Menu,
}
