using System.Collections.Generic;
using UnityEngine;

public class UIStateManager : MonoBehaviour
{
    public static UIStateManager Instance;

    #region States
    private UIManagerBaseState CurrentState { get; set; }
    public UIManagerTransitioningState ManagerTransitioningState { get; set; }
    public UIManagerLoadedState ManagerLoadedState { get; set; }
    public UIManagerUnLoadedState ManagerUnLoadedState { get; set; }
    #endregion

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
            if (FindInteractableUI(value) != null)
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
                        if (FindInteractableUI(new Vector2(temp, value.y)))
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
                        if (FindInteractableUI(new Vector2(value.x, temp)))
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

    public bool KeyOrControlActive { get; set; }
    public GameObject PauseInstance { get; set; }
    public GameObject CurrentUIPrefab { get; private set; }
    public List<GameObject> ListOfUIObjects { get; private set; }
    public float ResolutionScaling { get; private set; }
    public bool PlayTransition { get; set; }
    public bool ChangingSlider { get; set; }
    public GameObject PrefabToEnable { get; set; }
    public GameObject PrefabToDisable { get; set; }
    public GameObject ActivePrefab { get; set; }
    public bool PauseMenuActive { get; set; }
    public bool Transitioning { get; set; }
    public GameObject CursorInstance { get; private set; }

    private Vector2 prevPosition;
    public GameObject pausePrefab;

    public GameObject prefabWillEnableOnStart;

    public float maxXPos;
    public float maxYPos;


    private void Awake()
    {
        Time.maximumDeltaTime = 10;

        if (Instance == null)
            Instance = this;
        else
            Destroy(this);
    }

    private void Start()
    {
        CursorInstance = GameObject.FindGameObjectWithTag("Cursor");

        ManagerTransitioningState = new();
        ManagerUnLoadedState = new();
        ManagerLoadedState = new();

        ListOfUIObjects = new();

        ResolutionScaling = GetComponentInParent<Canvas>().scaleFactor;

        if (prefabWillEnableOnStart != null)
        {
            PlayTransition = true;
            PrefabToEnable = prefabWillEnableOnStart;
            CurrentState = ManagerLoadedState;
        }
        else
        {
            CurrentState = ManagerUnLoadedState;

            if (CoverManager.Instance != null)
            {
                CoverManager.Instance.UnCover(1, null);
            }
        }

        CurrentState.EnterState(this);
    }

    private void Update()
    {
        TransitionSystem.Update();

        CurrentState.UpdateState(this);
    }

    public void SwitchState(UIManagerBaseState state)
    {
        CurrentState.ExitState(this);

        CurrentState = state;

        CurrentState.EnterState(this);
    }

    public GameObject FindInteractableUI(Vector2 value)
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

    public void DisableUIPrefab()
    {
        PrefabToDisable.SetActive(false);
    }

    public void EnableUIPrefab()
    {
        PrefabToEnable.SetActive(true);
    }
    public void EnableTransitioning()
    {
        Transitioning = true;
    }

    public void DisableTransitioning()
    {
        Transitioning = false;
    }
}
