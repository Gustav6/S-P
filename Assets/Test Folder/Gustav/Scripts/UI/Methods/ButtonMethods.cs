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

    public bool prefabScaleTransition;
    public bool prefabMoveTransition;

    [SerializeField] private GameObject prefab;

    [SerializeField] bool unPause;
    [SerializeField] bool exit;

    private readonly float shrinkTime = 1.5f;
    private readonly float growTime = 1.5f;

    void Start()
    {
        if (prefabScaleTransition)
        {
            prefabMoveTransition = false;
        }
        if (!prefabMoveTransition && !prefabScaleTransition)
        {
            prefabScaleTransition = true;
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
            UIManager.InstantiateNewUIPrefab(prefab, transform.parent.parent, Vector3.one);
        }
        else if (prefabScaleTransition)
        {
            UIManager.InstantiateNewUIPrefab(prefab, transform.parent.parent, new Vector3(0.001f, 0.001f, 1));
            GrowTransition();
        }
    }

    public void ShrinkTransition(Transition.ActionDelegate @delegate)
    {
        for (int i = 0; i < UIManager.ListOfUIObjects.Count; i++)
        {
            GameObject temp = UIManager.ListOfUIObjects[i].gameObject;

            if (i == 0)
            {
                TransitionSystem.AddScaleTransition(new ScaleTransition(temp.transform, Vector3.zero, shrinkTime, TransitionType.SmoothStop2, @delegate));
            }
            else
            {
                TransitionSystem.AddScaleTransition(new ScaleTransition(temp.transform, Vector3.zero, shrinkTime, TransitionType.SmoothStop2));
            }
        }
    }

    public void GrowTransition()
    {
        for (int i = 0; i < UIManager.ListOfUIObjects.Count; i++)
        {
            GameObject temp = UIManager.ListOfUIObjects[i].gameObject;

            TransitionSystem.AddScaleTransition(new ScaleTransition(temp.transform, Vector3.one, growTime, TransitionType.SmoothStop2));
        }
    }
}

public enum NewScene
{
    Game,
    Menu,
}
