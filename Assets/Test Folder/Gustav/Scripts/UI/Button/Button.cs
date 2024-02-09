using System.Collections;
using System.Collections.Generic;
using UnityEditor.Experimental.GraphView;
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
    public GameObject prefabToSpawn;

    GameObject activePrefab;

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
    public void InstantiatePrefab()
    {
        PrefabDirection direction = PrefabDirection.None;
        Transform parent = null;

        if (prefabToSpawn.GetComponent<ActiveMenuManager>() != null)
        {
            direction = prefabToSpawn.GetComponent<ActiveMenuManager>().moveDirection;
            parent = activePrefab.transform.parent;
        }
        else if (prefabToSpawn.GetComponent<ActiveSettingManager>() != null)
        {
            direction = prefabToSpawn.GetComponent<ActiveSettingManager>().moveDirection;
            parent = activePrefab.transform.parent;
        }

        Destroy(activePrefab);

        if (prefabMoveTransition)
        {
            Vector3 spawnLocation = GiveDestination(direction) * -1;

            UIManager.instance.InstantiateNewUIPrefab(prefabToSpawn, parent, Vector3.one, spawnLocation);
            MoveUIToStart(1, UIManager.instance.DisableTransitioning, direction);
        }
        else if (prefabScaleTransition)
        {
            UIManager.instance.InstantiateNewUIPrefab(prefabToSpawn, parent, new Vector3(0.0001f, 0.0001f, 1), Vector3.zero);
            GrowTransition(1, UIManager.instance.DisableTransitioning);
        }
    }

    public void MoveUIToStart(float time, ExecuteOnCompletion actions, PrefabDirection direction)
    {
        actions += Test;

        activePrefab = GameObject.FindGameObjectWithTag("ActiveSettingPrefab");

        for (int i = 0; i < UIManager.instance.CurrentUIPrefab.transform.childCount; i++)
        {
            Vector3 destination = GiveDestination(direction);

            MoveGameObject(UIManager.instance.CurrentUIPrefab.transform.GetChild(i).gameObject, time, destination, i, actions);
        }

        //for (int i = 0; i < UIManager.instance.ListOfUIObjects.Count; i++)
        //{
        //    Vector3 destination = GiveDestination(direction);

        //    MoveGameObject(UIManager.instance.ListOfUIObjects[i], time, destination, i, actions);
        //}
    }

    public void Test()
    {
        List<GameObject> tempList = new();

        GameObject g = GameObject.FindGameObjectWithTag("UIPrefab");

        for (int i = 0; i < g.GetComponentsInChildren<BaseStateManager>().Length; i++)
        {
            tempList.Add(g.GetComponentsInChildren<BaseStateManager>()[i].gameObject);
        }

        UIManager.instance.CurrentUIPrefab.GetComponentInParent<UIManager>().LoadUI(tempList);
    }

    public void MoveThenDestoryUI(float time, ExecuteOnCompletion actions, PrefabDirection direction)
    {
        if (prefabToSpawn.GetComponent<ActiveMenuManager>() != null)
        {
            activePrefab = GameObject.FindGameObjectWithTag("UIPrefab");

            for (int i = 0; i < UIManager.instance.ListOfUIObjects.Count; i++)
            {
                Vector3 destination = GiveDestination(direction);

                MoveGameObject(UIManager.instance.ListOfUIObjects[i], time, destination, i, actions);
            }
        }
        else if (prefabToSpawn.GetComponent<ActiveSettingManager>() != null)
        {
            activePrefab = GameObject.FindGameObjectWithTag("ActiveSettingPrefab");

            for (int i = 0; i < activePrefab.transform.childCount; i++)
            {
                Vector3 destination = GiveDestination(direction);

                MoveGameObject(activePrefab.transform.GetChild(i).gameObject, time, destination, i, actions);
            }
        }

        //for (int i = 0; i < UIManager.instance.ListOfUIObjects.Count; i++)
        //{
        //    Vector3 destination = GiveDestination(direction);

        //    MoveGameObject(UIManager.instance.ListOfUIObjects[i], time, destination, actions);
        //}
    }

    //private Transform[] ReturnAllChildren(GameObject prefab)
    //{
    //    for (int i = 0; i <prefab.)
    //    Transform[] gameObjectArray = prefab.transform.GetChild(0);
    //    gameObjectArray[0] = null;

    //    return gameObjectArray;
    //}

    private void MoveGameObject(GameObject g, float time, Vector3 destination, int i, ExecuteOnCompletion execute = null, float windUp = 0, float overShoot = 0)
    {
        if (g != null && i == 0 && execute != null)
            TransitionSystem.AddMoveTransition(new MoveTransition(g.transform, destination, time, TransitionType.SmoothStop2, true, windUp, overShoot, execute));
        else if (g != null && i > 0)
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
        for (int i = 0; i < UIManager.instance.ListOfUIObjects.Count; i++)
        {
            GameObject temp = UIManager.instance.ListOfUIObjects[i];
            ScaleGameObject(temp, Vector3.zero, time, i, actions);
        }
    }

    public void GrowTransition(float time, Transition.ExecuteOnCompletion actions)
    {
        for (int i = 0; i < UIManager.instance.ListOfUIObjects.Count; i++)
        {
            GameObject temp = UIManager.instance.ListOfUIObjects[i];
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
}

public enum NewScene
{
    Game,
    Menu,
}
