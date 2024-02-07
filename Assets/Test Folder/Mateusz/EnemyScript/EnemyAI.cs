using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pathfinding;

public class EnemyAI : MonoBehaviour
{
    public Transform target;

    public float speed = 200f;
    public float flipSpeed;
    public float nextWayPointDistance = 3f;

    Path path;
    Seeker seeker;
    Rigidbody2D rb;
    Animator anim;

    public SpriteRenderer spriteRenderer;

    int currentWayPoint = 0;
    bool reachedDestination = false;
    void Start()
    {
        seeker = GetComponent<Seeker>();
        rb = GetComponent<Rigidbody2D>();

        InvokeRepeating("UpdatePath", 0f, .2f);
    }

    private void Awake()
    {
        anim = transform.GetChild(0).GetComponent<Animator>();
    }

    public void Update()
    {
        //    Vector2 direction = (target.position - transform.position).normalized;
        //    spriteRenderer.flipX = direction.x < 0;
        //    rb.velocity = direction * speed * Time.deltaTime;
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
    void FixedUpdate()
    {
        

        if (path == null)
        {
            reachedDestination = true;
            return;
        }
        else
        {
            reachedDestination = false;
        }
       

        Vector2 direction = ((Vector2)path.vectorPath[currentWayPoint] - rb.position).normalized;
        Vector2 force = (direction * speed * Time.deltaTime);

        rb.AddForce(force);

        float distance = Vector2.Distance(rb.position, path.vectorPath[currentWayPoint]);

        if (distance < nextWayPointDistance)
        {
            currentWayPoint++;
        }

        if (rb.velocity.x > 0)
        {
            spriteRenderer.flipX = false;
        }
        else if (rb.velocity.x < 0)
        {
            spriteRenderer.flipX = true;    
        }

        if (speed > 190)
        {
            anim.SetInteger("Velocity", 1);
        }
        else if (speed < 190)
        {
            anim.SetInteger("Velocity", 0);
        }
    }
}
