using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ButtonMethods : MonoBehaviour
{
    [SerializeField] private bool transitionToScene;
    public bool TransitionToScene { get { return transitionToScene; } }

    [SerializeField] private NewScene scene;

    [SerializeField] private bool transitionToPrefab;
    public bool TransitionToPrefab { get { return transitionToPrefab; } }

    [SerializeField] private GameObject prefab;

    [SerializeField] bool unPause;
    [SerializeField] bool exit;

    void Start()
    {

    }

    void Update()
    {
        
    }

    public void MethodsToRun()
    {
        if (transitionToScene)
        {
            SwitchScene();
        }

        if (transitionToPrefab)
        {
            InstantiatePrefab();
        }
    }

    private void SwitchScene()
    {
        SceneManager.LoadScene((int)scene);
    }

    private void InstantiatePrefab()
    {
        Destroy(gameObject.GetComponentInParent<ActiveMenuManager>().transform.gameObject);
        UIManager.InstantiateNewUIPrefab(prefab, transform.parent.parent);
    }
}

public enum NewScene
{
    Game,
    Menu,
}
