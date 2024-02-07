using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Button : UI
{
    private ButtonStateManager ButtonStateManager { get; set; }

    public bool transitionToScene;
    public NewScene NewScene;

    public bool transitionToPrefab;
    public bool prefabScaleTransition;
    public bool prefabMoveTransition;

    public GameObject menu;

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

            UIManager.InstantiateNewUIPrefab(menu, parentTransform, Vector3.one, spawnLocation);
            MoveUIToStart(menu.transform, 1, UIManager.DisableTransitioning, direction);
        }
        else if (prefabScaleTransition)
        {
            UIManager.InstantiateNewUIPrefab(menu, currentMenu.transform, new Vector3(0.0001f, 0.0001f, 1), Vector3.zero);
            GrowTransition(UIManager.DisableTransitioning, 1);
        }
    }

    public void MoveUIToStart(Transform t, float time, Transition.ExecuteOnCompletion actions, PrefabDirection direction)
    {
        foreach (Transform child in t)
        {
            Vector3 destination = GiveDestination(direction);

            MoveGameObject(child, destination, time);
        }

        StartCoroutine(WaitCoroutine(time, actions));
    }

    public void MoveThenDestoryUI(Transform t, float time, Transition.ExecuteOnCompletion actions, PrefabDirection direction)
    {
        foreach (Transform child in t)
        {
            Vector3 destination = GiveDestination(direction);

            MoveGameObject(child, destination, time);
        }

        StartCoroutine(WaitCoroutine(time, actions));
    }

    private void MoveGameObject(Transform t, Vector3 destination, float time, float windUp = 0, float overShoot = 0)
    {
        TransitionSystem.AddMoveTransition(new MoveTransition(t, destination, time, TransitionType.SmoothStop2, true, windUp, overShoot));
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
    public void ShrinkTransition(Transition.ExecuteOnCompletion actions, float time)
    {
        for (int i = 0; i < UIManager.ListOfUIObjects.Count; i++)
        {
            GameObject temp = UIManager.ListOfUIObjects[i].gameObject;
            ScaleGameObject(temp, Vector3.zero, time, i, actions);
        }
    }

    public void GrowTransition(Transition.ExecuteOnCompletion actions, float time)
    {
        for (int i = 0; i < UIManager.ListOfUIObjects.Count; i++)
        {
            GameObject temp = UIManager.ListOfUIObjects[i].gameObject;
            ScaleGameObject(temp, Vector3.one, time, i, actions);
        }
    }

    private void ScaleGameObject(GameObject g, Vector3 newScale, float time, float i, Transition.ExecuteOnCompletion executeOnCompletion = null)
    {
        if (i == 0)
            TransitionSystem.AddScaleTransition(new ScaleTransition(g.transform, newScale, time, TransitionType.SmoothStop2, executeOnCompletion));
        else
            TransitionSystem.AddScaleTransition(new ScaleTransition(g.transform, newScale, time, TransitionType.SmoothStop2));
    }
    #endregion

    private IEnumerator WaitCoroutine(float time, Transition.ExecuteOnCompletion actions)
    {
        yield return new WaitForSeconds(time);

        actions?.Invoke();
    }
}

public enum NewScene
{
    Game,
    Menu,
}
