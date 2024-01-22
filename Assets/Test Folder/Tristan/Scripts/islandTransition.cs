using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class islandTransition : MonoBehaviour
{
    public AnimationCurve curve;

    float timer = 0;
    float timerMax = 1;
    bool isSwitching = false;

    void Start()
    {

    }

    void Update()
    {
        timer += Time.deltaTime;

        Vector3 tempVector = transform.position;
        if (isSwitching == true)
        {
            if (transform.position.x <= 18)
            {
                tempVector.x = Mathf.Lerp(0, 18, curve.Evaluate(timer / timerMax));
            }
            else
            {

            }
            isSwitching = false;
        }

        transform.position = tempVector;
    }

    public void SwapIsland()
    {
        isSwitching = true;
    }
}
