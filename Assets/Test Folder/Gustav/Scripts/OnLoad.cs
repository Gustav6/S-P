using UnityEngine;
using UnityEngine.UI;

public class OnLoad : MonoBehaviour
{
    [Header("Drag image here")]
    public Image image;

    [Header("Fade in in variables")]
    public bool fadeIn;
    public Color color;
    public float fadeInTime = 1.5f;

    [Header("Move in variables")]
    public bool moveIn;
    public bool baseDestinationFromScreen;
    public float moveInTime = 1;
    public PrefabMoveDirection moveTowardsOnInstantiate;
    public PrefabMoveDirection moveTowardsOnDestroy;

    private Transition.ExecuteOnCompletion execute;
    private PrefabMoveDirection tempDirection;
    private GameObject instance;

    [Header("Scale in variables")]
    public bool scaleIn;
    public float scaleInTime = 1;
    public float scaleInResetTime = 1;
    public Vector3 newScale;


    public void Start()
    {
        tempDirection = moveTowardsOnDestroy;
        instance = gameObject;

        if (fadeIn)
        {
            Fade(image, fadeInTime, color);
        }
        else if (moveIn)
        {
            MoveIn(execute, moveInTime);
        }
        else if (scaleIn)
        {
            Scale();
        }
    }

    public void MoveIn(Transition.ExecuteOnCompletion execute, float time)
    {
        UIManager.Instance.EnableTransitioning();

        execute += UIManager.Instance.DisableTransitioning;

        if (GetComponent<ActiveBaseManager>() != null)
        {
            if (baseDestinationFromScreen)
            {
                Vector3 startingPosition = UIManager.Instance.GiveDestination(moveTowardsOnInstantiate) * -1;
                instance.transform.position += startingPosition;
            }
            else
            {
                Vector3 startingPosition = UIManager.Instance.GiveDestination(moveTowardsOnInstantiate) * -1;
                startingPosition.x += Screen.width / 2;
                startingPosition.y += Screen.height / 2;
                instance.transform.position = startingPosition;
            }
        }

        UIManager.Instance.MoveUIToStart(time, instance, execute);
    }

    public void MoveAway(GameObject g, float time, Transition.ExecuteOnCompletion execute)
    {
        UIManager.Instance.EnableTransitioning();

        execute += UIManager.Instance.DisableTransitioning;

        Vector3 destination = Vector3.zero;

        if (g.transform.localPosition.y >= Screen.height || g.transform.localPosition.x >= Screen.width)
        {
            if (tempDirection == PrefabMoveDirection.Down || tempDirection == PrefabMoveDirection.Up)
            {
                tempDirection = PrefabMoveDirection.Down;
            }
            else
            {
                tempDirection = PrefabMoveDirection.Left;
            }
        }
        else if (g.transform.localPosition.y <= -Screen.height || g.transform.localPosition.x <= -Screen.width)
        {
            if (tempDirection == PrefabMoveDirection.Up || tempDirection == PrefabMoveDirection.Down)
            {
                tempDirection = PrefabMoveDirection.Up;
            }
            else
            {
                tempDirection = PrefabMoveDirection.Right;
            }
        }

        destination = UIManager.Instance.GiveDestination(tempDirection);

        UIManager.Instance.MoveUITowardsDestination(g, time, destination, execute);
    }

    public void Scale()
    {
        Transition.ExecuteOnCompletion execute = ResetScale;

        TransitionSystem.AddScaleTransition(new ScaleTransition(transform, newScale, scaleInTime, TransitionType.SmoothStop2, execute));
    }

    public void ResetScale()
    {
        TransitionSystem.AddScaleTransition(new ScaleTransition(transform, Vector3.one, scaleInResetTime, TransitionType.SmoothStop2));
    }

    public void Fade(Image image, float fadeInTIme, Color newColor)
    {
        TransitionSystem.AddColorTransition(new ColorTransition(image, newColor, fadeInTIme, TransitionType.SmoothStart2));
    }
}
