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

public class UIManager : MonoBehaviour
{
    #region Variables 
    private bool keyOrControlActive;
    public bool KeyOrControlActive { get { return keyOrControlActive; } set { keyOrControlActive = value; } }

    [SerializeField] private GameObject currentUiElement;
    public GameObject CurrentUiElement { get { return currentUiElement; } set { currentUiElement = value; } }

    private Vector2 mousePosition;
    public Vector2 MousePosition { get { return mousePosition; } set { mousePosition = value; } }

    [SerializeField] private float resolutionScaling;
    public float ResolutionScaling { get {  return resolutionScaling; } set {  resolutionScaling = value; } }

    [SerializeField] private bool transitioning;
    public bool Transitioning { get { return transitioning; } }

    [SerializeField] private Vector2 currentUISelected;
    public Vector2 CurrentUISelected
    {
        get
        {
            return currentUISelected;
        }

        set
        {
            #region Value control
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

            if (CheckForGameObject(value) != null)
            {
                currentUISelected = value;
            }

            if (CheckForGameObject(value) == null && value.y > prevPosition.y || CheckForGameObject(value) == null && value.y < prevPosition.y)
            {
                float temp = 0;

                if (value.y < prevPosition.y)
                {
                    temp = maxXPos;
                }

                for (int i = 0; i < listOfUIObjects.Count; i++)
                {
                    if (CheckForGameObject(new Vector2(temp, value.y)))
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

            if (CheckForGameObject(value) == null && value.x > prevPosition.x || CheckForGameObject(value) == null && value.x < prevPosition.x)
            {
                float temp = 0;

                if (value.x < prevPosition.x)
                {
                    temp = maxYPos;
                }

                for (int i = 0; i < listOfUIObjects.Count; i++)
                {
                    if (CheckForGameObject(new Vector2(value.x, temp)))
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
            #endregion

            prevPosition = currentUISelected;
        }
    }

    private Vector2 prevPosition;

    [SerializeField] private GameObject pausePrefab;
    public GameObject PausePrefab { get { return pausePrefab; } }

    private float maxXPos;
    private float maxYPos;
    #endregion

    #region Static variables
    private static List<GameObject> listOfUIObjects = new();
    public static List<GameObject> ListOfUIObjects { get { return listOfUIObjects; } }

    private static GameObject currentUIGameObject;
    public static GameObject CurrentUIGameObject { get { return currentUIGameObject; } }
    #endregion

    void Awake()
    {
        LoadUI();
    }

    void Start()
    {
        resolutionScaling = GetComponentInParent<Canvas>().scaleFactor;
    }

    void Update()
    {
        TransitionSystem.Update();

        if (keyOrControlActive)
        {
            CheckMouseMovement();
        }
    }

    public void LoadUI(List<GameObject> list = null)
    {
        #region Reset variables
        maxXPos = 0;
        maxYPos = 0;
        listOfUIObjects.Clear();
        #endregion

        if (list != null)
        {
            FindEveryUIElement(list);
        }
        else
        {
            FindEveryUIElement(new List<GameObject>(GameObject.FindGameObjectsWithTag("InteractableUI")));
        }
    }

    private void FindEveryUIElement(List<GameObject> list)
    {
        // Note that if there is no start position (0, 0) bug starts.
        #region Find objects with script UI and max X & Y value
        foreach (GameObject interactableUI in list)
        {
            if (interactableUI.GetComponent<UI>().position == Vector2.zero)
            {
                currentUiElement = interactableUI;
            }

            if (interactableUI.GetComponent<UI>().position.y > maxYPos)
            {
                maxYPos = interactableUI.GetComponent<UI>().position.y;
            }

            if (interactableUI.GetComponent<UI>().position.x > maxXPos)
            {
                maxXPos = interactableUI.GetComponent<UI>().position.x;
            }

            listOfUIObjects.Add(interactableUI);
        }
        #endregion
    }

    public void EnableTransitioning()
    {
        transitioning = true;
    }

    public void DisableTransitioning()
    {
        transitioning = false;
    }

    public static void InstantiateNewUIPrefab(GameObject prefab, Transform parrentObject, Vector3 scale)
    {
        currentUIGameObject = Instantiate(prefab);
        currentUIGameObject.transform.SetParent(parrentObject);
        currentUIGameObject.transform.localScale = Vector3.one;
        currentUIGameObject.transform.localPosition = Vector3.zero;

        List<GameObject> tempList = new();

        for (int i = 0; i < currentUIGameObject.GetComponentsInChildren<UI>().Length; i++)
        {
            tempList.Add(currentUIGameObject.GetComponentsInChildren<UI>()[i].gameObject);
            currentUIGameObject.GetComponentsInChildren<UI>()[i].transform.localScale = scale;
        }

        currentUIGameObject.GetComponentInParent<UIManager>().LoadUI(tempList);
    }

    private GameObject CheckForGameObject(Vector2 value)
    {
        foreach (GameObject interactableUI in listOfUIObjects)
        {
            if (interactableUI.GetComponent<UI>() != null && interactableUI.GetComponent<UI>().position == value)
            {
                return interactableUI;
            }
        }

        return null;
    }

    private void CheckMouseMovement()
    {
        if (Mouse.current.delta.x.ReadValue() != 0 || Mouse.current.delta.y.ReadValue() != 0)
        {
            if (currentUiElement.GetComponent<UI>())
            {
                currentUiElement.GetComponent<UI>().activated = false;
            }

            keyOrControlActive = false;
        }
    }

    public bool HoveringGameObject(GameObject g)
    {
        if (g.GetComponent<Collider2D>().OverlapPoint(mousePosition))
        {
            if (!transitioning)
            {
                currentUiElement = g;
                if (g.GetComponent<UI>() != null)
                {
                    currentUISelected = g.GetComponent<UI>().position;
                }
                return true;
            }
        }

        return false;
    }
}
