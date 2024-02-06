using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class UIActions : MonoBehaviour
{
    public static void SwitchScene(NewScene scene)
    {
        SceneManager.LoadScene((int)scene);
    }

    public static void InstantiatePrefab(bool prefabMove, bool prefabScale, PrefabDirection direction, GameObject prefab, GameObject currentMenu)
    {
        Destroy(currentMenu);

        if (prefabMove)
        {
            Vector3 spawnLocation = GiveDestination(direction) * -1;

            UIManager.InstantiateNewUIPrefab(prefab, currentMenu.transform, Vector3.one, spawnLocation);
            MovePrefabToStart(UIManager.DisableTransitioning, direction, 1);
        }
        else if (prefabScale)
        {
            UIManager.InstantiateNewUIPrefab(prefab, currentMenu.transform, new Vector3(0.0001f, 0.0001f, 1), Vector3.zero);
            GrowTransition(UIManager.DisableTransitioning, 1);
        }
    }

    #region Move Transition
    public static void MovePrefabToStart(Transition.ExecuteOnCompletion actions, PrefabDirection direction, float time)
    {
        for (int i = 0; i < UIManager.ListOfUIObjects.Count; i++)
        {
            GameObject temp = UIManager.ListOfUIObjects[i].gameObject;

            Vector3 destination = GiveDestination(direction);

            MoveGameObjects(temp, destination, time, i, actions);
        }
    }

    public static void MovePrefabToDestination(Transition.ExecuteOnCompletion actions, PrefabDirection direction, float time)
    {
        for (int i = 0; i < UIManager.ListOfUIObjects.Count; i++)
        {
            GameObject temp = UIManager.ListOfUIObjects[i].gameObject;

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

    private static Vector3 GiveDestination(PrefabDirection direction)
    {
        if (direction == PrefabDirection.Left)
        {
            return new(-Screen.width, 0, 0);
        }
        else if (direction == PrefabDirection.Right)
        {
            return new(Screen.width, 0, 0);
        }

        return Vector3.zero;
    }
    #endregion

    #region Scale Transition
    public static void ShrinkTransition(Transition.ExecuteOnCompletion actions, float time)
    {
        for (int i = 0; i < UIManager.ListOfUIObjects.Count; i++)
        {
            GameObject temp = UIManager.ListOfUIObjects[i].gameObject;
            ScaleGameObjects(temp, Vector3.zero, time, i, actions);
        }
    }

    public static void GrowTransition(Transition.ExecuteOnCompletion actions, float time)
    {
        for (int i = 0; i < UIManager.ListOfUIObjects.Count; i++)
        {
            GameObject temp = UIManager.ListOfUIObjects[i].gameObject;
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
