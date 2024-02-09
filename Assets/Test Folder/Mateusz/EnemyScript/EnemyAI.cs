using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pathfinding;
using System.Runtime.CompilerServices;

public class EnemyAI : MonoBehaviour
{
    private Transform target;

    [SerializeField] private GameObject hitbox;

    [SerializeField] private float speed = 2;
    [SerializeField] private float flipSpeed;
    public float nextWayPointDistance = 3f;

    Path path;
    Seeker seeker;
    Rigidbody2D rb;

    int currentWayPoint = 0;

    private int _previousDirection = 1;

    public bool CanMove { get; set; }

    void Start()
    {
        seeker = GetComponent<Seeker>();
        rb = GetComponent<Rigidbody2D>();

        target = PlayerStats.Instance.transform;

        CanMove = true;

        InvokeRepeating("UpdatePath", 0f, .2f);
    }

    #region A*
    void UpdatePath()
    {
        if (seeker.IsDone())
        {
            seeker.StartPath(rb.position, target.position, OnPathComplete);
        }
    }

    private void OnPathComplete(Path p)
    {
        if (!p.error)
        {
            path = p;
            currentWayPoint = 0;
        }
    }
    #endregion
    
    void Update()
    {
        Vector2 direction = ((Vector2)target.position - (Vector2)transform.position).normalized;
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        hitbox.transform.localRotation = Quaternion.AngleAxis(angle, Vector3.forward);

        if (path == null)
        {
            return;
        }

        if (!CanMove)
            return;

        Vector2 pathDirection = ((Vector2)path.vectorPath[currentWayPoint] - rb.position).normalized;
        Vector2 force = (pathDirection * speed);

        rb.velocity = force;

        float distance = Vector2.Distance(rb.position, path.vectorPath[currentWayPoint]);

        if (distance < nextWayPointDistance)
        {
            currentWayPoint++;
        }

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
            transform.localScale = new Vector2(Mathf.Lerp(0.5f * -direction, 0.5f * direction, time / flipSpeed), 0.5f);
            time += Time.deltaTime;
            yield return null;
        }

        transform.localScale = new Vector3(0.5f * direction, 0.5f, 0.5f);
    }
}
