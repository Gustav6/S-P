using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor.Experimental.GraphView;
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

    public void StartSceneTransition()
    {
        ExecuteOnCompletion execute = null;

        if (moveTransition)
        {
            MoveUIThenRemove(transitionDuration, null);
            execute += SwitchScene;
        }
        else if (scaleTransition)
        {
            ShrinkTransition(transitionDuration, null);
            execute += SwitchScene;
        }
        else
        {
            execute += SwitchScene;
        }

        PanelManager.FadeOut(transitionDuration, new Color(0, 0, 0, 1), execute);
        Debug.Log("Change Scene");
    }

    public void StartPrefabTransition()
    {
        ExecuteOnCompletion execute = null;

        ActiveMenuManager currentMenu = GetComponentInParent<ActiveMenuManager>();

        if (moveTransition)
        {
            //execute += InstantiatePrefab;
            MoveUIThenRemove(transitionDuration, execute);
            Debug.Log("Move In Prefab");
        }
        else if (scaleTransition)
        {
            //execute += InstantiatePrefab;
            ShrinkTransition(transitionDuration, execute);
            Debug.Log("Scale In Prefab");
        }
    }

    private void SwitchScene()
    {
        SceneManager.LoadScene((int)NewScene);
    }

    #region Move Transition
    private void InstantiateNewPrefab()
    {
        Transform parent = UIManager.instance.gameObject.transform;

        List<UI> childrenToRemove = new();

        for (int i = 0; i < activePrefab.transform.childCount; i++)
        {
            if (activePrefab.transform.GetChild(i).GetComponent<UI>() != null)
            {
                childrenToRemove.Add(activePrefab.transform.GetChild(i).GetComponent<UI>());
            }
            else if (activePrefab.transform.GetChild(i).GetComponent<ActiveSettingManager>() != null)
            {
                for (int y = 0; y < activePrefab.transform.GetChild(i).transform.childCount; y++)
                {
                    if (activePrefab.transform.GetChild(y).GetComponent<UI>() != null)
                    {
                        childrenToRemove.Add(activePrefab.transform.GetChild(y).GetComponent<UI>());
                    }
                }
            }
        }

        if (prefabToSpawn.GetComponent<ActiveSettingManager>() != null)
        {
            parent = UIManager.instance.GetComponentInChildren<ActiveMenuManager>().transform;
        }

        DestroyUI(activePrefab, childrenToRemove);

        if (moveTransition)
        {
            Vector3 spawnLocation = GiveDestination(GiveDirection(prefabToSpawn));

            UIManager.instance.InstantiateNewUIPrefab(prefabToSpawn, parent, Vector3.one, spawnLocation);
            MoveUIToStart(1, UIManager.instance.DisableTransitioning);
        }
        else if (scaleTransition)
        {
            UIManager.instance.InstantiateNewUIPrefab(prefabToSpawn, parent, new Vector3(0.0001f, 0.0001f, 1), Vector3.zero);
            GrowTransition(1, UIManager.instance.DisableTransitioning);
        }

        UIManager.instance.LoadUI();
    }

    private void MoveUIToStart(float time, ExecuteOnCompletion actions)
    {
        activePrefab = GameObject.FindGameObjectWithTag("ActiveSettingPrefab");

        for (int i = 0; i < UIManager.instance.CurrentUIPrefab.transform.childCount; i++)
        {
            Vector3 destination = GiveDestination(GiveDirection(UIManager.instance.CurrentUIPrefab)) * -1;

            MoveGameObject(UIManager.instance.CurrentUIPrefab.transform.GetChild(i).gameObject, time, destination, i, actions);
        }
    }

    private void MoveUIThenRemove(float time, ExecuteOnCompletion actions)
    {
        actions += InstantiateNewPrefab;

        if (prefabToSpawn != null && prefabToSpawn.GetComponent<ActiveMenuManager>() != null)
        {
            activePrefab = GameObject.FindGameObjectWithTag("UIPrefab");

            for (int i = 0; i < UIManager.instance.ListOfUIObjects.Count; i++)
            {
                Vector3 destination = GiveDestination(GiveDirection(prefabToSpawn));

                MoveGameObject(UIManager.instance.ListOfUIObjects[i], time, destination, i, actions);
            }
        }
        else if (prefabToSpawn != null && prefabToSpawn.GetComponent<ActiveSettingManager>() != null)
        {
            activePrefab = GameObject.FindGameObjectWithTag("ActiveSettingPrefab");

            for (int i = 0; i < activePrefab.transform.childCount; i++)
            {
                Vector3 destination = GiveDestination(GiveDirection(prefabToSpawn));

                MoveGameObject(activePrefab.transform.GetChild(i).gameObject, time, destination, i, actions);
            }
        }
        else
        {
            for (int i = 0; i < UIManager.instance.ListOfUIObjects.Count; i++)
            {
                Vector3 destination = GiveDestination(PrefabMoveDirection.Left);

                MoveGameObject(UIManager.instance.ListOfUIObjects[i], time, destination, i, actions);
            }
        }
    }

    private void LoadNewUI()
    {
        List<GameObject> tempList = new();

        for (int i = 0; i < UIManager.instance.GetComponentsInChildren<BaseStateManager>().Length; i++)
        {
            tempList.Add(UIManager.instance.transform.GetChild(i).gameObject);

            Debug.Log(UIManager.instance.GetComponentInChildren<BaseStateManager>().gameObject);
        }

        UIManager.instance.LoadUI(tempList);
    }

    private void MoveGameObject(GameObject g, float time, Vector3 destination, int i, ExecuteOnCompletion execute = null, float windUp = 0, float overShoot = 0)
    {
        if (g != null && i == 0)
            TransitionSystem.AddMoveTransition(new MoveTransition(g.transform, destination, time, TransitionType.SmoothStop2, true, windUp, overShoot, execute));
        else if (g != null && i > 0)
            TransitionSystem.AddMoveTransition(new MoveTransition(g.transform, destination, time, TransitionType.SmoothStop2, true, windUp, overShoot));
    }   

    private Vector3 GiveDestination(PrefabMoveDirection direction)
    {
        if (direction == PrefabMoveDirection.Left)
            return new(-Screen.width, 0, 0);
        else if (direction == PrefabMoveDirection.Right)
            return new(Screen.width, 0, 0);
        else
            return Vector3.zero;
    }

    private PrefabMoveDirection GiveDirection(GameObject g)
    {
        if (g.GetComponent<ActiveMenuManager>() != null)
        {
            return PrefabMoveDirection.Left;
        }
        else if (g.GetComponent<ActiveSettingManager>() != null)
        {
            return PrefabMoveDirection.Right;
        }

        return PrefabMoveDirection.None;
    }
    #endregion

    #region Scale Transition
    private void ShrinkTransition(float time, ExecuteOnCompletion actions)
    {
        for (int i = 0; i < UIManager.instance.CurrentUIPrefab.transform.childCount; i++)
        {
            GameObject temp = UIManager.instance.ListOfUIObjects[i];
            ScaleGameObject(temp, Vector3.zero, time, i, actions);
        }
    }

    private void GrowTransition(float time, ExecuteOnCompletion actions)
    {
        actions += LoadNewUI;

        activePrefab = GameObject.FindGameObjectWithTag("ActiveSettingPrefab");

        for (int i = 0; i < UIManager.instance.CurrentUIPrefab.transform.childCount; i++)
        {
            GameObject temp = UIManager.instance.ListOfUIObjects[i];
            ScaleGameObject(temp, Vector3.one, time, i, actions);
        }
    }

    private void ScaleGameObject(GameObject g, Vector3 newScale, float time, float i, Transition.ExecuteOnCompletion execute = null)
    {
        if (g != null && i == 0 && execute != null)
            TransitionSystem.AddScaleTransition(new ScaleTransition(g.transform, newScale, time, TransitionType.SmoothStop2, execute));
        else if (g != null && i > 0)
            TransitionSystem.AddScaleTransition(new ScaleTransition(g.transform, newScale, time, TransitionType.SmoothStop2));
    }
    #endregion

    public void DestroyUI(GameObject prefab, List<UI> activeUI)
    {
        for (int i = 0; i < activeUI.Count; i++)
        {
            activeUI[i].IsDestroyed = true;
        }

        Destroy(prefab);
    }
}

public enum NewScene
{
    Game,
    Menu,
}
