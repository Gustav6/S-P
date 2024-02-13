using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class IslandTransition : MonoBehaviour
{
    public AnimationCurve curve;

    float timerMax = 1;

    public void Start()
    {
        SwapIsland();
    }
    public void SwapIsland()
    {
        StartCoroutine(MoveIsland());
    }

    IEnumerator MoveIsland()
    {
        float startPos = transform.position.x;
        float timer = 0;

        while (timer < timerMax)
        {
            transform.position = new Vector2(Mathf.Lerp(startPos, startPos + 18, curve.Evaluate(timer / timerMax)), 0);
            yield return null;
            timer += Time.deltaTime;
        }
        transform.position = new Vector2(startPos + 18, 0);

        if (transform.position.x == 18)
        {
            Destroy(gameObject);
        }
    }
}
