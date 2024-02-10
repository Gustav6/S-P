using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class _Test : MonoBehaviour
{
    [SerializeField] private GameObject spawn;

    private GameObject go;

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Z) && go == null)
            go = Instantiate(spawn, Vector3.zero, Quaternion.identity);
        else if (Input.GetKeyDown(KeyCode.Z))
        {
            Destroy(go);
            go = Instantiate(spawn, Vector3.zero, Quaternion.identity);
        }
    }
}
