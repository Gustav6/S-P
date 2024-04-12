using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;

public class Bobbing : MonoBehaviour
{
    [SerializeField] private bool scale;
    [SerializeField] private bool move;

    [SerializeField] private float speed;
    [SerializeField] private float magnitude;
    [SerializeField] private float offset;

    public GameObject left;
    public GameObject right;

    private float xOffsetLeft;
    private float xOffsetRight;

    public void Start()
    {
        if (move)
        {
            left = transform.GetChild(0).gameObject;
            right = transform.GetChild(1).gameObject;

            xOffsetLeft = left.transform.localPosition.x;
            xOffsetRight = right.transform.localPosition.x;
        }
    }

    void Update()
    {
        if (scale)
        {
            Scale();
        }

        if (move)
        {
            BobLeftToRight();
        }
    }

    public void BobLeftToRight()
    {
        Vector3 tempLeft = new(-SinCurve() + xOffsetLeft, 0);
        Vector3 tempRight = new(SinCurve() + xOffsetRight, 0);
        left.transform.localPosition = tempLeft;
        right.transform.localPosition = tempRight;
    }

    public void Scale()
    {
        Vector3 scale = new(SinCurve() + offset, SinCurve() + offset);
        transform.localScale = scale;
    }

    public float SinCurve()
    {
        return magnitude * MathF.Sin(Time.time * speed);
    }
}
