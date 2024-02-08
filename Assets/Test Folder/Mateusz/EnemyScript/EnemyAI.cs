using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pathfinding;

public class EnemyAI : MonoBehaviour
{
    public Transform target;

    public float speed = 0.5f;
    public float flipSpeed;
    public float nextWayPointDistance = 3f;

    Path path;
    Seeker seeker;
    Rigidbody2D rb;

    public SpriteRenderer spriteRenderer;

    int currentWayPoint = 0;
    bool reachedDestination = false;
    
    public bool CanMove { get; set; }

    Animator anim;

    void Start()
    {
        seeker = GetComponent<Seeker>();
        rb = GetComponent<Rigidbody2D>();

        CanMove = true;

        InvokeRepeating("UpdatePath", 0f, .2f);
    }

    private void Awake()
    {
        anim = transform.GetChild(0).GetComponent<Animator>();
    }

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

    // Update is called once per frame
    void Update()
    {

        if (path == null)
        {
            return;
        }

        if (currentWayPoint >= path.vectorPath.Count)
        {
            reachedDestination = true;
            return;
        }
        else
        {
            reachedDestination = false;
        }

        if (!CanMove)
            return;

        Vector2 direction = ((Vector2)path.vectorPath[currentWayPoint] - rb.position).normalized;
        Vector2 force = (direction * speed);

        rb.velocity = force;

        float distance = Vector2.Distance(rb.position, path.vectorPath[currentWayPoint]);

        if (distance < nextWayPointDistance)
        {
            currentWayPoint++;
        }

        // TODO: Ändra sen
        if (rb.velocity.x > 0)
        {
            transform.localScale = new Vector2(0.5f, 0.5f);
        }
        else if (rb.velocity.x < 0)
        {
            transform.localScale = new Vector2(-0.5f, 0.5f);
        }
    }
}
