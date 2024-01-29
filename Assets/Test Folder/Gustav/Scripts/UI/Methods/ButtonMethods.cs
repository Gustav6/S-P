using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class ButtonMethods : MonoBehaviour
{
    [SerializeField] private bool transitionToScene;
    public bool TransitionToScene { get { return transitionToScene; } }

    [SerializeField] private NewScene scene;

    [SerializeField] private bool transitionToPrefab;
    public bool TransitionToPrefab { get { return transitionToPrefab; } }

    private bool prefabScaleTransition;
    public bool PrefabScaleTransition { get { return prefabScaleTransition; } }

    private bool prefabMoveTransition;
    public bool PrefabMoveTransition { get { return prefabMoveTransition; } }

    [SerializeField] private GameObject prefab;

    private ActiveMenuManager activeMenuManager;

    [SerializeField] bool unPause;
    [SerializeField] bool exit;

    private readonly float shrinkTime = 1.5f;
    private readonly float growTime = 1.5f;
    private readonly float moveToDesDuration = 1.05f;
    private readonly float moveToStartDuration = 0.8f;

    void Start()
    {
        activeMenuManager = GetComponentInParent<ActiveMenuManager>();

        if (activeMenuManager.whereWillPrefabMove == MoveTowards.None)
        {
            prefabMoveTransition = false;
            prefabScaleTransition = true;
        }
        else
        {
            prefabMoveTransition = true;
            prefabScaleTransition = false;
        }
    }

    public void SwitchScene()
    {
        SceneManager.LoadScene((int)scene);
    }

    public void InstantiatePrefab()
    {
        Destroy(gameObject.GetComponentInParent<ActiveMenuManager>().transform.gameObject);

        if (prefabMoveTransition)
        {
            Vector3 spawnLocation = GiveDestination() * -1;

            UIManager.InstantiateNewUIPrefab(prefab, transform.parent.parent, Vector3.one, spawnLocation);
            MovePrefabToStart();
        }
        else if (prefabScaleTransition)
        {
            UIManager.InstantiateNewUIPrefab(prefab, transform.parent.parent, new Vector3(0.0001f, 0.0001f, 1), Vector3.zero);
            GrowTransition();
        }
    }

    #region Move Transition
    public void MovePrefabToStart()
    {
        for (int i = 0; i < UIManager.ListOfUIObjects.Count; i++)
        {
            GameObject temp = UIManager.ListOfUIObjects[i].gameObject;

            Vector3 destination = GiveDestination();

            Transition.ExecuteOnCompletion actions = null;
            actions += UIManager.DisableTransitioning;

            MoveGameObjects(temp, destination, moveToStartDuration, i, actions);
        }
    }

    public void MovePrefabToDestination()
    {
        for (int i = 0; i < UIManager.ListOfUIObjects.Count; i++)
        {
            GameObject temp = UIManager.ListOfUIObjects[i].gameObject;

            Vector3 destination = GiveDestination();

            Transition.ExecuteOnCompletion actions = null;
            actions += InstantiatePrefab;

            MoveGameObjects(temp, destination, moveToDesDuration, i, actions);
        }
    }

    public void MoveGameObjects(GameObject g, Vector3 destination, float time, float i, Transition.ExecuteOnCompletion executeOnCompletion = null)
    {
        if (i == 0)
        {
            TransitionSystem.AddMoveTransition(new MoveTransition(g.transform, destination, time, TransitionStart.SmoothStart2, TransitionEnd.SmoothStop2, true, executeOnCompletion));
        }
        else
        {
            TransitionSystem.AddMoveTransition(new MoveTransition(g.transform, destination, time, TransitionStart.SmoothStart2, TransitionEnd.SmoothStop2, true));
        }
    }

    public Vector3 GiveDestination()
    {
        if (activeMenuManager.whereWillPrefabMove == MoveTowards.Left)
        {
            return new(-Screen.width, 0, 0);
        }
        else if (activeMenuManager.whereWillPrefabMove == MoveTowards.Right)
        {
            return new(Screen.width, 0, 0);
        }

        return Vector3.zero;
    }
    #endregion

    #region Scale Transition
    public void ShrinkTransition()
    {
        for (int i = 0; i < UIManager.ListOfUIObjects.Count; i++)
        {
            GameObject temp = UIManager.ListOfUIObjects[i].gameObject;

            Transition.ExecuteOnCompletion actions = null;
            actions += InstantiatePrefab;

            ScaleGameObjects(temp, Vector3.zero, shrinkTime, i, actions);
        }
    }

    public void GrowTransition()
    {
        for (int i = 0; i < UIManager.ListOfUIObjects.Count; i++)
        {
            GameObject temp = UIManager.ListOfUIObjects[i].gameObject;

            Transition.ExecuteOnCompletion actions = null;
            actions += UIManager.DisableTransitioning;

            ScaleGameObjects(temp, Vector3.one, growTime, i, actions);
        }
    }

    public void ScaleGameObjects(GameObject g, Vector3 newScale, float time, float i, Transition.ExecuteOnCompletion executeOnCompletion = null)
    {
        if (i == 0)
        {
            TransitionSystem.AddScaleTransition(new ScaleTransition(g.transform, newScale, time, TransitionType.SmoothStop2, executeOnCompletion));
        }
        else
        {
            TransitionSystem.AddScaleTransition(new ScaleTransition(g.transform, newScale, time, TransitionType.SmoothStop2));
        }
    }
    #endregion
}

public enum NewScene
{
    Game,
    Menu,
}
