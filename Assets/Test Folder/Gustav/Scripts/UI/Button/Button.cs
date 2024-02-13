using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using static Transition;

public class Button : UI
{
    [Header("Button variables")]
    private ButtonStateManager ButtonStateManager { get; set; }

    [SerializeField] private List<Functions> selectedFunctions = new();
    private Dictionary<Functions, System.Action> functionLookup;

    [Range(0.1f, 5)] public float transitionDuration;

    public ActiveScene NewScene;

    [Range(-2, 0)] public float windUp;
    [Range(0, 2)] public float overShoot;

    [Header("Drag in the prefab here")]
    public GameObject prefabToSpawn;

    private void Awake()
    {
        functionLookup = new Dictionary<Functions, System.Action>()
        {
            { Functions.SwitchScene, SceneTransition },
            { Functions.RemovePrefab, RemovePrefab },
            { Functions.SpawnPrefab, null },
            { Functions.QuitGame, Application.Quit }
        };
    }

    private ExecuteOnCompletion execute = null;

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

    private void SceneTransition()
    {
        execute += SwitchScene;

        PanelManager.FadeOut(transitionDuration, new Color(0, 0, 0, 1), execute);
    }

    private void RemovePrefab()
    {
        if (selectedFunctions.Contains(Functions.SpawnPrefab))
        {
            execute += UIManager.instance.InstantiateNewPrefab;
        }

        UIManager.instance.MoveUIThenRemove(transitionDuration, prefabToSpawn, execute, windUp, overShoot);
    }

    private void SwitchScene()
    {
        SceneManager.LoadScene((int)NewScene);
    }

    public void ActiveSelectedfunction()
    {
        for (int i = 0; i < selectedFunctions.Count; i++)
        {
            functionLookup[selectedFunctions[i]]?.Invoke();
        }
    }

    private enum Functions
    {
        SwitchScene,
        RemovePrefab,
        SpawnPrefab,
        QuitGame,
    }
}

public enum ActiveScene
{
    MainMenu,
    Game,
}
