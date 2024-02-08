using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using static Transition;

public class Button : UI
{
    private ButtonStateManager ButtonStateManager { get; set; }

    public bool transitionToScene;
    public NewScene NewScene;

    public bool transitionToPrefab;
    public bool prefabScaleTransition;
    public bool prefabMoveTransition;

    [Header("Drag in the prefab here")]
    public GameObject menuPrefab;

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

    public void SwitchScene()
    {
        SceneManager.LoadScene((int)NewScene);
    }

    #region Move Transition
    public void InstantiatePrefab(PrefabDirection direction, GameObject currentMenu, Transform parentTransform)
    {
        Destroy(currentMenu);

        if (prefabMoveTransition)
        {
            Vector3 spawnLocation = GiveDestination(direction) * -1;

            UIManager.InstantiateNewUIPrefab(menuPrefab.gameObject, parentTransform, Vector3.one, spawnLocation);
            MoveUIToStart(1, UIManager.DisableTransitioning, direction);
        }
        else if (prefabScaleTransition)
        {
            UIManager.InstantiateNewUIPrefab(menuPrefab, currentMenu.transform, new Vector3(0.0001f, 0.0001f, 1), Vector3.zero);
            GrowTransition(1, UIManager.DisableTransitioning);
        }
    }

    public void MoveUIToStart(float time, ExecuteOnCompletion actions, PrefabDirection direction)
    {
        actions += UIManager.DisableTransitioning;

        for (int i = 0; i < UIManager.ListOfUIObjects.Count; i++)
        {
            Vector3 destination = GiveDestination(direction);

            MoveGameObject(UIManager.ListOfUIObjects[i].gameObject, time, destination, i, actions);
        }
    }

    public void MoveThenDestoryUI(float time, ExecuteOnCompletion actions, PrefabDirection direction)
    {
        for (int i = 0; i < UIManager.ListOfUIObjects.Count; i++)
        {
            Vector3 destination = GiveDestination(direction);

            MoveGameObject(UIManager.ListOfUIObjects[i].gameObject, time, destination, i, actions);
        }
    }

    private void MoveGameObject(GameObject g, float time, Vector3 destination, int i, ExecuteOnCompletion execute = null, float windUp = 0, float overShoot = 0)
    {
        if (i == 0 && execute != null)
            TransitionSystem.AddMoveTransition(new MoveTransition(g.transform, destination, time, TransitionType.SmoothStop2, true, windUp, overShoot, execute));
        else
            TransitionSystem.AddMoveTransition(new MoveTransition(g.transform, destination, time, TransitionType.SmoothStop2, true, windUp, overShoot));
    }

    private Vector3 GiveDestination(PrefabDirection direction)
    {
        if (direction == PrefabDirection.Left)
            return new(-Screen.width, 0, 0);
        else if (direction == PrefabDirection.Right)
            return new(Screen.width, 0, 0);
        else
            return Vector3.zero;
    }
    #endregion

    #region Scale Transition
    public void ShrinkTransition(float time, ExecuteOnCompletion actions)
    {
        for (int i = 0; i < UIManager.ListOfUIObjects.Count; i++)
        {
            GameObject temp = UIManager.ListOfUIObjects[i].gameObject;
            ScaleGameObject(temp, Vector3.zero, time, i, actions);
        }
    }

    public void GrowTransition(float time, Transition.ExecuteOnCompletion actions)
    {
        for (int i = 0; i < UIManager.ListOfUIObjects.Count; i++)
        {
            GameObject temp = UIManager.ListOfUIObjects[i].gameObject;
            ScaleGameObject(temp, Vector3.one, time, i, actions);
        }
    }

    private void ScaleGameObject(GameObject g, Vector3 newScale, float time, float i, Transition.ExecuteOnCompletion execute = null)
    {
        if (i == 0 && execute != null)
            TransitionSystem.AddScaleTransition(new ScaleTransition(g.transform, newScale, time, TransitionType.SmoothStop2, execute));
        else
            TransitionSystem.AddScaleTransition(new ScaleTransition(g.transform, newScale, time, TransitionType.SmoothStop2));
    }
    #endregion

    private IEnumerator WaitCoroutine(float time, Transition.ExecuteOnCompletion actions)
    {
        Debug.Log("Startred");

        yield return new WaitForSeconds(time);

        Debug.Log("Done");

        actions?.Invoke();
    }
}

public enum NewScene
{
    Game,
    Menu,
}
