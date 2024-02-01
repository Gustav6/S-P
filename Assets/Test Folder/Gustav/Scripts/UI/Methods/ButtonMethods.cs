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

    [SerializeField] private bool prefabScaleTransition;
    public bool PrefabScaleTransition { get { return prefabScaleTransition; } }

    [SerializeField] private bool prefabMoveTransition;
    public bool PrefabMoveTransition { get { return prefabMoveTransition; } }

    [SerializeField] private GameObject prefab;

    private ActiveMenuManager activeMenuManager;

    [SerializeField] bool unPause;
    [SerializeField] bool exit;

    void Start()
    {
        activeMenuManager = GetComponentInParent<ActiveMenuManager>();

        if (!prefabMoveTransition && !prefabScaleTransition)
        {
            prefabMoveTransition = true;
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
            MovePrefabToStart(UIManager.DisableTransitioning, 1);
        }
        else if (prefabScaleTransition)
        {
            UIManager.InstantiateNewUIPrefab(prefab, transform.parent.parent, new Vector3(0.0001f, 0.0001f, 1), Vector3.zero);
            GrowTransition(UIManager.DisableTransitioning, 1);
        }
    }

    #region Move Transition
    public void MovePrefabToStart(Transition.ExecuteOnCompletion actions, float time)
    {
        for (int i = 0; i < UIManager.ListOfUIObjects.Count; i++)
        {
            GameObject temp = UIManager.ListOfUIObjects[i].gameObject;

            Vector3 destination = GiveDestination();

            MoveGameObjects(temp, destination, time, i, actions);
        }
    }

    public void MovePrefabToDestination(Transition.ExecuteOnCompletion actions, float time)
    {
        for (int i = 0; i < UIManager.ListOfUIObjects.Count; i++)
        {
            GameObject temp = UIManager.ListOfUIObjects[i].gameObject;

            Vector3 destination = GiveDestination();

            MoveGameObjects(temp, destination, time, i, actions);
        }
    }

    public void MoveGameObjects(GameObject g, Vector3 destination, float time, float i, Transition.ExecuteOnCompletion executeOnCompletion = null, float windUp = 0, float overShoot = 0)
    {
        if (i == 0)
        {
            TransitionSystem.AddMoveTransition(new MoveTransition(g.transform, destination, time, TransitionType.SmoothStop2, true, executeOnCompletion));
        }
        else
        {
            TransitionSystem.AddMoveTransition(new MoveTransition(g.transform, destination, time, TransitionType.SmoothStop2, true, executeOnCompletion));
        }
    }

    private Vector3 GiveDestination()
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
    public void ShrinkTransition(Transition.ExecuteOnCompletion actions, float time)
    {
        for (int i = 0; i < UIManager.ListOfUIObjects.Count; i++)
        {
            GameObject temp = UIManager.ListOfUIObjects[i].gameObject;
            ScaleGameObjects(temp, Vector3.zero, time, i, actions);
        }
    }

    public void GrowTransition(Transition.ExecuteOnCompletion actions, float time)
    {
        for (int i = 0; i < UIManager.ListOfUIObjects.Count; i++)
        {
            GameObject temp = UIManager.ListOfUIObjects[i].gameObject;
            ScaleGameObjects(temp, Vector3.one, time, i, actions);
        }
    }

    private void ScaleGameObjects(GameObject g, Vector3 newScale, float time, float i, Transition.ExecuteOnCompletion executeOnCompletion = null)
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
