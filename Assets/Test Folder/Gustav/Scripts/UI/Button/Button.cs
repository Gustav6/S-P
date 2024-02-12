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
    public bool moveTransition;

    [Range(-2, 0)] public float windUp;
    [Range(0, 2)] public float overShoot;

    [Header("Drag in the prefab here")]
    public GameObject prefabToSpawn;

    public bool quit;

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
        UIManager.instance.MoveUIThenRemove(transitionDuration, null, windUp, overShoot);
        Debug.Log("Move In Prefab");
    }

    public void QuitGame()
    {
        Application.Quit();
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
