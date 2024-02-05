using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;

public class Bobbing : MonoBehaviour
{
    [SerializeField] private float speed;
    [SerializeField] private float magnitude;

    public GameObject left;
    public GameObject right;

    private float xOffsetLeft;
    private float xOffsetRight;

    public void Start()
    {
        left = transform.GetChild(0).gameObject;
        right = transform.GetChild(1).gameObject;

        xOffsetLeft = left.transform.localPosition.x;
        xOffsetRight = right.transform.localPosition.x;
    }

    void Update()
    {
        Vector3 tempLeft = new(-SinCurve() + xOffsetLeft, 0);
        Vector3 tempRight = new(SinCurve() + xOffsetRight, 0);
        left.transform.localPosition = tempLeft;
        right.transform.localPosition = tempRight;
    }

    public float SinCurve()
    {
        return magnitude * MathF.Sin(Time.time * speed);
    }
}
