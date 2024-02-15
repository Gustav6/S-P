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
    private Dictionary<Functions, System.Action> functionLookup;

    [Range(0.1f, 5)] public float transitionDuration;

    public ActiveScene NewScene;

    [Range(-2, 0)] public float windUp;
    [Range(0, 2)] public float overShoot;

    [Header("Drag in the prefab here")]
    public GameObject prefabToSpawn;

    public override void Start()
    {
        functionLookup = new Dictionary<Functions, System.Action>()
        {
            { Functions.SwitchScene, SceneTransition },
            { Functions.RemovePrefab, RemovePrefab },
            { Functions.SpawnPrefab, null },
            { Functions.QuitGame, Application.Quit },
            { Functions.SwipeTransition, PlaySwipeTransition }
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

    private void SceneTransition()
    {
        ExecuteOnCompletion execute = SwitchScene;

        PanelManager.FadeOut(transitionDuration, new Color(0, 0, 0, 1), execute);
    }

    private void RemovePrefab()
    {
        ExecuteOnCompletion execute = null;

        if (selectedFunctions.Contains(Functions.SpawnPrefab))
        {
            execute += UIManager.Instance.TempInstantiateNewPrefab;
        }

        UIManager.Instance.MoveUIThenRemove(transitionDuration, prefabToSpawn, execute);
    }

    private void SwitchScene()
    {
        SceneManager.LoadScene((int)NewScene);
    }

    public void ActivateSelectedFunctions()
    {
        for (int i = 0; i < selectedFunctions.Count; i++)
        {
            functionLookup[selectedFunctions[i]]?.Invoke();
        }
    }

    public void PlaySwipeTransition()
    {
        if (PanelManager.Instance != null)
        {
            ExecuteOnCompletion execute = RevealNewPrefab;

            PanelManager.Instance.OnLoadInstance.MoveAway(PanelManager.Instance.gameObject, transitionDuration, execute);
        }
    }

    private void RevealNewPrefab()
    {
        ExecuteOnCompletion execute = UIManager.Instance.DisableTransitioning;
        PanelManager.Instance.OnLoadInstance.MoveAway(PanelManager.Instance.gameObject, transitionDuration, execute);
    }

    private enum Functions
    {
        SwitchScene,
        RemovePrefab,
        SpawnPrefab,
        QuitGame,
        SwipeTransition,
    }
}

public enum ActiveScene
{
    MainMenu,
    Game,
}
