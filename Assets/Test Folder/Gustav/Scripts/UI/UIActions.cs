using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class UIActions : MonoBehaviour
{

    #region Move Transition
    public static void MovePrefabToStart(Transition.ExecuteOnCompletion actions, PrefabMoveDirection direction, float time)
    {
        for (int i = 0; i < UIManager.instance.ListOfUIObjects.Count; i++)
        {
            GameObject temp = UIManager.instance.ListOfUIObjects[i];

            Vector3 destination = GiveDestination(direction);

            MoveGameObjects(temp, destination, time, i, actions);
        }
    }

    public static void MovePrefabToDestination(Transition.ExecuteOnCompletion actions, PrefabMoveDirection direction, float time)
    {
        for (int i = 0; i < UIManager.instance.ListOfUIObjects.Count; i++)
        {
            GameObject temp = UIManager.instance.ListOfUIObjects[i];

            Vector3 destination = GiveDestination(direction);

            MoveGameObjects(temp, destination, time, i, actions);
        }
    }

    private static void MoveGameObjects(GameObject g, Vector3 destination, float time, float i, Transition.ExecuteOnCompletion executeOnCompletion = null, float windUp = 0, float overShoot = 0)
    {
        if (i == 0)
        {
            TransitionSystem.AddMoveTransition(new MoveTransition(g.transform, destination, time, TransitionType.SmoothStop2, true, windUp, overShoot, executeOnCompletion));
        }
        else
        {
            TransitionSystem.AddMoveTransition(new MoveTransition(g.transform, destination, time, TransitionType.SmoothStop2, true, windUp, overShoot));
        }
    }

    private static Vector3 GiveDestination(PrefabMoveDirection direction)
    {
        if (direction == PrefabMoveDirection.Left)
        {
            return new(-Screen.width, 0, 0);
        }
        else if (direction == PrefabMoveDirection.Right)
        {
            return new(Screen.width, 0, 0);
        }

        return Vector3.zero;
    }
    #endregion

    #region Scale Transition
    public static void ShrinkTransition(Transition.ExecuteOnCompletion actions, float time)
    {
        for (int i = 0; i < UIManager.instance.ListOfUIObjects.Count; i++)
        {
            GameObject temp = UIManager.instance.ListOfUIObjects[i];
            ScaleGameObjects(temp, Vector3.zero, time, i, actions);
        }
    }

    public static void GrowTransition(Transition.ExecuteOnCompletion actions, float time)
    {
        for (int i = 0; i < UIManager.instance.ListOfUIObjects.Count; i++)
        {
            GameObject temp = UIManager.instance.ListOfUIObjects[i];
            ScaleGameObjects(temp, Vector3.one, time, i, actions);
        }
    }

    private static void ScaleGameObjects(GameObject g, Vector3 newScale, float time, float i, Transition.ExecuteOnCompletion executeOnCompletion = null)
    {
        if (i == 0)
        {
            TransitionSystem.AddScaleTransition(new ScaleTransition(g.transform, newScale, time, TransitionType.SmoothStop2, executeOnCompletion));
        }
        else
        {
            TransitionSystem.AddScaleTransition(new ScaleTransition(g.transform, newScale, time, TransitionType.SmoothStop2));
        }
    }
    #endregion
}
