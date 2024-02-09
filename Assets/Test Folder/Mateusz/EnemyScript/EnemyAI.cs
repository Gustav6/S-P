using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pathfinding;
using System.Runtime.CompilerServices;

public class EnemyAI : MonoBehaviour
{
    private Transform target;

    [SerializeField] private GameObject pivot;

    [SerializeField] private float speed = 2;
    [SerializeField] private float flipSpeed;

    Rigidbody2D rb;

    private int _previousDirection = 1;

    public bool CanMove { get; set; }
    public bool IsNotGettingHit { get; set; }

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();

        target = PlayerStats.Instance.transform;

        CanMove = true;
    }
    
    void Update()
    {
        if (!CanMove)
        {
            if (IsNotGettingHit)
                rb.velocity = Vector2.zero;

            return;
        }

        Vector2 direction = ((Vector2)target.position - (Vector2)transform.position).normalized;
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        angle *= Mathf.Sign(transform.localScale.x);
        pivot.transform.localRotation = Quaternion.AngleAxis(angle, Vector3.forward);

        rb.velocity = direction * speed;

        // Negative here because of how sprite reacts to parent object being flipped.
        if (_previousDirection != -Mathf.Sign(transform.position.x - target.position.x))
        {
            _previousDirection = -(int)Mathf.Sign(transform.position.x - target.position.x);

            StartCoroutine(Flip(_previousDirection));
        }
    }

    private IEnumerator Flip(float direction)
    {
        float time = 0;

        while (time <= flipSpeed)
        {
            transform.localScale = new Vector3(Mathf.Lerp(-direction, direction, time / flipSpeed), 1, 1);
            time += Time.deltaTime;
            yield return null;
        }

        transform.localScale = new Vector3(direction, 1, 1);
    }
}
