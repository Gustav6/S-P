using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CursorScript : MonoBehaviour
{
    private void Start()
    {
        //Cursor.visible = false;
    }

    private void Update()
    {
        transform.position = new Vector3(Input.mousePosition.x, Input.mousePosition.y);
    }
}
