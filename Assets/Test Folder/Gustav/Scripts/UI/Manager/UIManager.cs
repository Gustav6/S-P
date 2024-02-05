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

public class UIManager : MonoBehaviour
{
    #region Variables 
    public bool KeyOrControlActive { get; set; }
    public bool ChangingSlider { get; set; }
    public Vector2 MousePosition { get; set; }
    public static float ResolutionScaling { get; private set; }

    public Vector2 currentUISelected;
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

    private Vector2 prevPosition;

    [SerializeField] private GameObject pausePrefab;
    public GameObject PausePrefab { get { return pausePrefab; } }

    [SerializeField] private float maxXPos;
    [SerializeField] private float maxYPos;
    #endregion

    #region Static variables
    public static List<GameObject> ListOfUIObjects { get; private set; }
    public static GameObject CurrentUIPrefab { get; private set; }
    public static bool Transitioning { get; private set; }
    public static DataManager DataManagerInstance { get; private set; }
    #endregion

    void Awake()
    {
        ListOfUIObjects = new();

        LoadUI();

        DataManagerInstance = GameObject.FindGameObjectWithTag("DataManager").GetComponent<DataManager>();
    }

    void Start()
    {
        ResolutionScaling = GetComponentInParent<Canvas>().scaleFactor;
    }

    void Update()
    {
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
            if (interactableUI.GetComponent<UI>().position == Vector2.zero)
            {
                currentUISelected = interactableUI.GetComponent<UI>().position;
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
        #endregion
    }

    public static void EnableTransitioning()
    {
        Transitioning = true;
    }

    public static void DisableTransitioning()
    {
        Transitioning = false;
    }

    public static void ResetListOfUIObjects()
    {
        ListOfUIObjects.Clear();
    }

    public static void InstantiateNewUIPrefab(GameObject prefab, Transform parentObject, Vector3 scale, Vector3 offset)
    {
        CurrentUIPrefab = Instantiate(prefab);
        CurrentUIPrefab.transform.SetParent(parentObject);
        CurrentUIPrefab.transform.localScale = Vector3.one;
        CurrentUIPrefab.transform.localPosition = Vector3.zero;

        List<GameObject> tempList = new();

        for (int i = 0; i < CurrentUIPrefab.GetComponentsInChildren<UI>().Length; i++)
        {
            tempList.Add(CurrentUIPrefab.GetComponentsInChildren<UI>()[i].gameObject);
            CurrentUIPrefab.GetComponentsInChildren<UI>()[i].transform.localScale = scale;
            CurrentUIPrefab.GetComponentsInChildren<UI>()[i].transform.position += offset;
        }

        CurrentUIPrefab.GetComponentInParent<UIManager>().LoadUI(tempList);
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

    public bool HoveringGameObject(GameObject g)
    {
        if (ListOfUIObjects.Count > 0 && !Transitioning)
        {
            if (g.GetComponent<Collider2D>().OverlapPoint(MousePosition))
            {
                if (g.GetComponent<UI>() != null)
                {
                    currentUISelected = g.GetComponent<UI>().position;
                    return true;
                }
            }
        }

        return false;
    }
}