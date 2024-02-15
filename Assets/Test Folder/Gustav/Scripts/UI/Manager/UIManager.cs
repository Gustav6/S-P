using NUnit.Framework;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System;
using static Transition;

public class UIManager : MonoBehaviour
{
    public static UIManager Instance { get; private set; }

    #region Variables 
    public bool KeyOrControlActive { get; set; }
    public bool ChangingSlider { get; set; }
    public Vector2 MousePosition { get; set; }
    public float ResolutionScaling { get; private set; }

    private Vector2 currentUISelected;
    public Vector2 CurrentUISelected
    {
        get
        {
            return currentUISelected;
        }

        set
        {
            #region Check max or min value
            if (value.y > maxYPos)
            {
                value.y = 0;
            }
            else if (value.y < 0)
            {
                value.y = maxYPos;
            }

            if (value.x > maxXPos)
            {
                value.x = 0;
            }
            else if (value.x < 0)
            {
                value.x = maxXPos;
            }
            #endregion

            #region Find next ui element
            if (CheckForInteractableUI(value) != null)
            {
                currentUISelected = value;
            }
            else
            {
                if (value.y > prevPosition.y || value.y < prevPosition.y)
                {
                    float temp = 0;

                    if (value.y < prevPosition.y)
                    {
                        temp = maxXPos;
                    }

                    for (int i = 0; i < ListOfUIObjects.Count; i++)
                    {
                        if (CheckForInteractableUI(new Vector2(temp, value.y)))
                        {
                            currentUISelected = new Vector2(temp, value.y);
                        }
                        else
                        {
                            if (value.y < prevPosition.y)
                            {
                                temp--;
                            }
                            else
                            {
                                temp++;
                            }
                        }
                    }
                }

                if (value.x > prevPosition.x || value.x < prevPosition.x)
                {
                    float temp = 0;

                    if (value.x < prevPosition.x)
                    {
                        temp = maxYPos;
                    }

                    for (int i = 0; i < ListOfUIObjects.Count; i++)
                    {
                        if (CheckForInteractableUI(new Vector2(value.x, temp)))
                        {
                            currentUISelected = new Vector2(value.x, temp);
                        }
                        else
                        {
                            if (value.x < prevPosition.x)
                            {
                                temp--;
                            }
                            else
                            {
                                temp++;
                            }
                        }
                    }
                }
            }

            
            #endregion

            prevPosition = currentUISelected;
        }
    }
    public List<GameObject> ListOfUIObjects { get; private set; }
    public GameObject CurrentUIPrefab { get; private set; }
    public bool Paused { get; set; }
    public GameObject CameraInstance { get; private set; }
    public bool Transitioning { get; private set; }

    private Vector2 prevPosition;

    public GameObject pausePrefab;

    private float maxXPos;
    private float maxYPos;

    #endregion

    public GameObject prefabToSpawn;

    private void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
            Destroy(this);

        CameraInstance = GameObject.FindGameObjectWithTag("MainCamera");

        CurrentUIPrefab = GameObject.FindGameObjectWithTag("UIPrefab");

        ListOfUIObjects = new();

        LoadUI();
    }

    private void Start()
    {
        ResolutionScaling = GetComponentInParent<Canvas>().scaleFactor;
    }

    private void Update()
    {
        Debug.Log(Transitioning);

        TransitionSystem.Update();

        if (KeyOrControlActive)
        {
            CheckForMouseMovement();
        }
    }

    public GameObject CheckForInteractableUI(Vector2 value)
    {
        foreach (GameObject interactableUI in ListOfUIObjects)
        {
            if (interactableUI.GetComponent<UI>().position == value)
            {
                return interactableUI;
            }
        }

        return null;
    }

    public void LoadUI(List<GameObject> list = null)
    {
        #region Reset variables
        maxXPos = 0;
        maxYPos = 0;
        ResetListOfUIObjects();
        #endregion

        if (list != null)
        {
            FindEveryInteractableUI(list);
        }
        else
        {
            FindEveryInteractableUI(new List<GameObject>(GameObject.FindGameObjectsWithTag("InteractableUI")));
        }
    }

    public void FindEveryInteractableUI(List<GameObject> list)
    {
        // Note that if there is no start position (0, 0) bugs will appear.
        #region Find objects with script UI and max X & Y value
        foreach (GameObject interactableUI in list)
        {
            if (!interactableUI.GetComponent<UI>().IsDestroyed)
            {
                if (interactableUI.GetComponent<UI>().position == Vector2.zero)
                {
                    CurrentUISelected = interactableUI.GetComponent<UI>().position;
                }

                if (interactableUI.GetComponent<UI>().position.y > maxYPos)
                {
                    maxYPos = interactableUI.GetComponent<UI>().position.y;
                }

                if (interactableUI.GetComponent<UI>().position.x > maxXPos)
                {
                    maxXPos = interactableUI.GetComponent<UI>().position.x;
                }

                ListOfUIObjects.Add(interactableUI);
            }
        }
        #endregion
    }

    public void EnableTransitioning()
    {
        Transitioning = true;
    }

    public void DisableTransitioning()
    {
        Transitioning = false;
    }

    public void ResetListOfUIObjects()
    {
        ListOfUIObjects.Clear();
    }

    public void InstantiateNewUIPrefab(GameObject prefab, Transform parentObject, Vector3 scale, Vector3 offset)
    {
        CurrentUIPrefab = Instantiate(prefab);
        CurrentUIPrefab.transform.SetParent(parentObject);
        CurrentUIPrefab.transform.localScale = scale;
        CurrentUIPrefab.transform.localPosition = offset;

        //ActiveMenuManager activePrefab = prefab.GetComponent<ActiveMenuManager>();

        //if (activePrefab.enableBlurOnInstantiate)
        //{
        //    activePrefab.EnableBlur(CameraInstance.GetComponent<Blur>());
        //}
        //else
        //{
        //    activePrefab.DisableBlur(CameraInstance.GetComponent<Blur>());
        //}
    }

    private void CheckForMouseMovement()
    {
        if (ListOfUIObjects.Count > 0)
        {
            if (Mouse.current.delta.x.ReadValue() != 0 || Mouse.current.delta.y.ReadValue() != 0)
            {
                KeyOrControlActive = false;
            }
        }
    }

    public bool Hovering(GameObject g)
    {
        if (ListOfUIObjects.Count > 0 && !Transitioning)
        {
            if (g.GetComponent<UI>() != null)
            {
                if (g.GetComponent<Collider2D>().OverlapPoint(MousePosition))
                {
                    return true;
                }
            }
        }

        return false;
    }

    public void TempInstantiateNewPrefab()
    {
        Transform parent = gameObject.transform;

        if (prefabToSpawn != null)
        {
            if (prefabToSpawn.GetComponent<ActiveMenuManager>() != null)
            {
                for (int i = 0; i < transform.childCount; i++)
                {
                    if (transform.GetChild(i).GetComponent<ActiveBaseManager>() != null)
                    {
                        DestroyUI(transform.GetChild(i).gameObject);
                    }
                }
            }
            else
            {
                if (CurrentUIPrefab != null)
                {
                    DestroyUI(CurrentUIPrefab);
                }
            }

            Vector3 spawnLocation = GiveDestination(GetInstantiateDirection(prefabToSpawn));

            InstantiateNewUIPrefab(prefabToSpawn, parent, Vector3.one, spawnLocation);
            MoveUIToStart(1, prefabToSpawn, DisableTransitioning);
            prefabToSpawn = null;

            List<GameObject> list = new();

            foreach (UI uI in GetComponentsInChildren<UI>())
            {
                if (!uI.IsDestroyed)
                {
                    list.Add(uI.gameObject);
                }
            }

            LoadUI(list);
        }
    }

    public void MoveUITowardsDestination(GameObject willMove, float time, Vector3 destination, ExecuteOnCompletion actions)
    {
        MoveGameObject(willMove, time, destination, actions);
    }

    public void MoveUIToStart(float time, GameObject willMove, ExecuteOnCompletion actions)
    {
        //Vector3 destination = GiveDestination(GetInstantiateDirection(g)) * -1;

        //Vector3 destination = new(Screen.width / 2, Screen.height / 2, 0);

        Vector3 destination = GiveDestination(GetInstantiateDirection(willMove));

        MoveGameObject(willMove, time, destination, actions, 0, 0);
    }

    public void MoveUIThenRemove(float time, GameObject _prefabToSpawn, ExecuteOnCompletion actions, float windUp = 0, float overShoot = 0)
    {
        prefabToSpawn = _prefabToSpawn;

        List<GameObject> temp = new();

        if (_prefabToSpawn.GetComponent<ActiveMenuManager>() != null)
        {
            foreach (ActiveBaseManager active in GetComponentsInChildren<ActiveBaseManager>())
            {
                Vector3 destination = GiveDestination(GetRemoveDirection(active.gameObject));

                MoveGameObject(active.gameObject, time, destination, actions, windUp, overShoot);
            }

            //for (int i = 0; i < transform.GetComponentsInChildren<ActiveBaseManager>().Length; i++)
            //{
            //    if (transform.GetChild(i).GetComponent<ActiveBaseManager>() != null)
            //    {
            //        temp.Add(transform.GetChild(i).GetComponent<ActiveBaseManager>().gameObject);

            //        Vector3 destination = GiveDestination(GetRemoveDirection(transform.GetChild(i).GetComponent<ActiveBaseManager>().gameObject));

            //        MoveGameObject(transform.GetChild(i).gameObject, time, destination, actions, windUp, overShoot);
            //    }
            //}
        }
        else if (_prefabToSpawn.GetComponent<ActiveSettingManager>() != null) 
        {
            if (GetComponentInChildren<ActiveSettingManager>() != null)
            {
                GameObject g = GetComponentInChildren<ActiveSettingManager>().gameObject;
                Vector3 destination = GiveDestination(GetRemoveDirection(g));

                MoveGameObject(g, time, destination, actions, windUp, overShoot);

                CurrentUIPrefab = g;
            }
        }


        //List<GameObject> willRemove = new();

        //if (uIPrefab.GetComponent<ActiveMenuManager>() != null)
        //{
        //    for (int i = 0; i < transform.childCount; i++)
        //    {
        //        if (transform.GetChild(i).GetComponent<ActiveBaseManager>() != null)
        //        {
        //            willRemove.Add(transform.GetChild(i).gameObject);
        //        }
        //    }
        //}

        //if (willRemove.Count > 1)
        //{
        //    for (int i = 0; i < willRemove.Count; i++)
        //    {
        //        Vector3 destination = GiveDestination(GetRemoveDirection(willRemove[i])) * -1;

        //        MoveGameObject(willRemove[i], time, destination, actions, windUp, overShoot);
        //    }
        //}
        //else
        //{
        //    Vector3 destination = GiveDestination(GetRemoveDirection(uIPrefab)) * -1;

        //    MoveGameObject(uIPrefab, time, destination, actions, windUp, overShoot);
        //}
    }

    private void MoveGameObject(GameObject g, float time, Vector3 destination, ExecuteOnCompletion execute = null, float windUp = 0, float overShoot = 0)
    {
        TransitionType type = TransitionType.SmoothStop2;
        TransitionSystem.AddMoveTransition(new MoveTransition(g.transform, destination, time, type, true, windUp, overShoot, execute));
    }

    public Vector3 GiveDestination(PrefabMoveDirection direction)
    {
        Vector3 temp = Vector3.zero;

        switch (direction)
        {
            case PrefabMoveDirection.Left:
                temp = new(-Screen.width, 0, 0);
                break;
            case PrefabMoveDirection.Right:
                temp = new(Screen.width, 0, 0);
                break;
            case PrefabMoveDirection.Up:
                temp = new(0, Screen.height, 0);
                break;
            case PrefabMoveDirection.Down:
                temp = new(0, -Screen.height);
                break;
            default:
                break;
        }

        return temp;
    }

    public GameObject GetActiveUIPrefab(GameObject prefab)
    {
        GameObject temp = null;

        if (prefab.GetComponent<ActiveSettingManager>() != null)
        {
            temp = GetComponentInChildren<ActiveSettingManager>().gameObject;
        }
        else if (prefab.GetComponent<ActiveMenuManager>() != null)
        {
            temp = GetComponentInChildren<ActiveMenuManager>().gameObject;
        }

        return temp;
    }

    private PrefabMoveDirection GetInstantiateDirection(GameObject g)
    {
        try
        {
            return g.GetComponent<OnLoad>().moveTowardsOnInstantiate;
        }
        catch (NullReferenceException)
        {
            Debug.Log("Cant get move direction");
            throw;
        }
    }
    private PrefabMoveDirection GetRemoveDirection(GameObject g)
    {
        try
        {
            return g.GetComponent<OnLoad>().moveTowardsOnDestroy;
        }
        catch (NullReferenceException)
        {
            Debug.Log("Cant get move direction");
            throw;
        }
    }

    public void DestroyUI(GameObject g)
    {
        foreach (UI uI in g.GetComponentsInChildren<UI>())
        {
            uI.IsDestroyed = true;
        }

        Destroy(g);
    }
}
