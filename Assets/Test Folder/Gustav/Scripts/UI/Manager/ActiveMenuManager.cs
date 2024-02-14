using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActiveMenuManager : ActiveBaseManager
{
    [Header("Objects will Instantiate")]
    public GameObject[] objects;
    public List<GameObject> instantiatedObjects;
    public bool enableBlur;

    public override void Start()
    {
        if (objects != null)
        {
            for (int i = 0; i < objects.Length; i++)
            {
                GameObject g = Instantiate(objects[i], GetComponentInParent<UIManager>().transform);
                instantiatedObjects.Add(g);
            }
        }
    }

    public void EnableBlur(Blur blurScript)
    {
        blurScript.radius = 3.6f;
        blurScript.qualityIterations = 2;
        blurScript.filter = 2;
    }

    public void DisableBlur(Blur blurScript)
    {
        blurScript.radius = 0;
        blurScript.qualityIterations = 1;
        blurScript.filter = 0;
    }

    public void OnDestroy()
    {
        for (int i = 0; i < instantiatedObjects.Count; i++)
        {
            Destroy(instantiatedObjects[i]);
        }
    }
}