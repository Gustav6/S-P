using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    #region Variables 
    private bool keyOrControlActive;
    public bool KeyOrControlActive { get { return keyOrControlActive; } set { keyOrControlActive = value; } }

    [SerializeField] private GameObject currentUIObject;
    public GameObject CurrentButten { get { return currentUIObject; } set { currentUIObject = value; } }

    private Vector2 mousePosition;
    public Vector2 MousePosition { get { return mousePosition; } set { mousePosition = value; } }

    [SerializeField] private float resolutionScaling;
    public float ResolutionScaling { get {  return resolutionScaling; } set {  resolutionScaling = value; } }

    private bool transitioning;
    public bool Transitioning { get { return transitioning; } set { transitioning = value; } }

    [SerializeField] private Vector2 currentUISelected;
    public Vector2 CurrentUISelected
    {
        get
        {
            return currentUISelected;
        }

        set
        {
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

                for (int i = 0; i < amountOfUIObjects.Count; i++)
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

                for (int i = 0; i < amountOfUIObjects.Count; i++)
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

            prevPosition = currentUISelected;
        }
    }

    [SerializeField] private Vector2 prevPosition;

    public static List<GameObject> amountOfUIObjects = new();

    private float maxXPos;
    private float maxYPos;
    #endregion

    void Awake()
    {
        foreach (GameObject interactableUI in GameObject.FindGameObjectsWithTag("InteractableUI"))
        {
            if (interactableUI.GetComponent<UI>().position == Vector2.zero)
            {
                currentUIObject = GetComponentInChildren<UI>().gameObject;
            }

            if (interactableUI.GetComponent<UI>().position.y > maxYPos)
            {
                maxYPos = interactableUI.GetComponent<UI>().position.y;
            }

            if (interactableUI.GetComponent<UI>().position.x > maxXPos)
            {
                maxXPos = interactableUI.GetComponent<UI>().position.x;
            }

            amountOfUIObjects.Add(interactableUI);
        }
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
            DetectMouseMovement();
        }

        if (transitioning)
        {
            for (int i = amountOfUIObjects.Count - 1; i >= 0; i--)
            {
                if (amountOfUIObjects[i].transform.localScale.x < 0.01f || amountOfUIObjects[i].transform.localScale.y < 0.01f)
                {
                    Destroy(amountOfUIObjects[i]);
                    amountOfUIObjects.RemoveAt(i);
                }
            }
        }
    }

    private GameObject CheckForGameObject(Vector2 value)
    {
        foreach (GameObject interactableUI in amountOfUIObjects)
        {
            if (interactableUI.GetComponent<UI>() != null && interactableUI.GetComponent<UI>().position == value)
            {
                return interactableUI;
            }
        }

        return null;
    }

    private void DetectMouseMovement()
    {
        if (Mouse.current.delta.x.ReadValue() != 0 || Mouse.current.delta.y.ReadValue() != 0)
        {
            if (currentUIObject.GetComponent<UI>())
            {
                currentUIObject.GetComponent<UI>().activated = false;
            }

            keyOrControlActive = false;
        }
    }

    public bool HoveringGameObject(GameObject g)
    {
        if (g.GetComponent<Collider2D>().OverlapPoint(mousePosition))
        {
            currentUIObject = g;
            if (g.GetComponent<UI>() != null)
            {
                currentUISelected = g.GetComponent<UI>().position;   
            }
            return true;
        }

        return false;
    }
}
