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

            currentUISelected = value;

        }
    }

    public static List<GameObject> amountOfUIObjects = new();

    private float maxXPos = 2;
    private float maxYPos = 2;
    #endregion

    void Awake()
    {
        foreach (GameObject interactableUI in GameObject.FindGameObjectsWithTag("InteractableUI"))
        {
            if (interactableUI.GetComponent<UI>().position == Vector2.zero)
            {
                currentUIObject = GetComponentInChildren<UI>().gameObject;
            }
                
            amountOfUIObjects.Add(interactableUI);
        }
    }

    private void Start()
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
