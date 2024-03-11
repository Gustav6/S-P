using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Transition;
using UnityEngine.SceneManagement;

public class InputField : UI
{
    public InputFieldStateManager InputFieldStateManager { get; private set; }

    [Header("Button variables")]
    [SerializeField] private List<Functions> selectedFunctions = new();
    private Dictionary<Functions, Action> functionLookup;

    [Range(0.1f, 5)] public float transitionDuration;

    public ActiveScene NewScene;
    public bool playTransition;

    [Range(1, 20)] public int maxAmountOfLetters;

    public override void Start()
    {
        functionLookup = new Dictionary<Functions, System.Action>()
        {
            { Functions.SwitchScene, SceneTransition },
        };

        InputFieldStateManager = GetComponent<InputFieldStateManager>();

        LoadFunction(InputFieldStateManager);

        base.Start();
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

    public void ActivateSelectedFunctions()
    {
        for (int i = 0; i < selectedFunctions.Count; i++)
        {
            functionLookup[selectedFunctions[i]]?.Invoke();
        }
    }

    public override void Update()
    {
        UpdateFunction(InputFieldStateManager);

        base.Update();
    }
    private enum Functions
    {
        SwitchScene,
    }
}
