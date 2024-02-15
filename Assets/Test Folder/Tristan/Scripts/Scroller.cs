using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Scroller : MonoBehaviour
{
    [SerializeField] float speed = 1;

    float startPos;

    void Update()
    {
        startPos = transform.localPosition.x;
        startPos += 0.0025f * speed;

        transform.localPosition = new Vector3(startPos, 0, 0);

        if (transform.localPosition.x >= 23.41f) {
            transform.localPosition = new Vector3(-23.41f, 0,0);
        }
    }
}
