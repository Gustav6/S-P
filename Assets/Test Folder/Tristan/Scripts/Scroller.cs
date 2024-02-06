using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Scroller : MonoBehaviour
{
    float startPos;
    void Update()
    {
        startPos = transform.position.x;
        startPos += 0.0025f;

        transform.position = new Vector3(startPos, 0, 0);

        if (transform.position.x >= 18)
        {
            transform.position = new Vector3(-18,0,0);
        }
    }
}
