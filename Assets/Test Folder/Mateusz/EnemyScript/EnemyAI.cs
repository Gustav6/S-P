using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pathfinding;
using System.Runtime.CompilerServices;
using System;

public class EnemyAI : MonoBehaviour
{
    private Transform target;

    [SerializeField] private GameObject pivot;

    [SerializeField] private float speed = 2;
    [SerializeField] private float flipSpeed;

    [SerializeField] private PauseController _pauseController;
    private bool _paused;

    private float nextWayPointDistance = 3f;

    Path path;
    Seeker seeker;
    Rigidbody2D rb;

    int currentWayPoint = 0;

    private int _previousDirection = 1;

    bool reachedEndOfPath;
    public bool CanMove { get; set; }
    public bool IsNotGettingHit { get; set; }

    void Start()
    {
        seeker = GetComponent<Seeker>();
        rb = GetComponent<Rigidbody2D>();

        target = PlayerStats.Instance.transform;

        Enemy enemy = GetComponent<Enemy>();

        if (enemy.waveMachine.CurrentState != enemy.waveMachine.States[WaveStateMachine.WaveState.WaveLoss])
            CanMove = true;
        else
        {
            CanMove = false;
        }

        InvokeRepeating("UpdatePath", 0f, .2f);
    }
    
    private void ToggleEnemyAction(object sender, EventArgs args)
    {
        _paused = ((OnPausedEventArgs)args).isPaused;
    }

    void UpdatePath()
    {
        if (!CanMove)
        {
            seeker.CancelCurrentPathRequest(false);
            return;
        }

        if (seeker.IsDone())
        {
            seeker.StartPath(rb.position, target.position, OnPathComplete);
        }
    }

    void OnPathComplete(Path p)
    {
        if (!p.error)
        {
            path = p;
            currentWayPoint = 0;
        }
    }

    void Update()
    {
        if (!CanMove)
        {
            if (IsNotGettingHit)
                rb.velocity = Vector2.zero;

            return;
        }

        if (path == null)
            return;

        if (currentWayPoint >= path.vectorPath.Count)
        {
            reachedEndOfPath = true;
            return;
        }
        else
        {
            reachedEndOfPath = false;
        }

        Vector2 direction = ((Vector2)target.position - (Vector2)transform.position).normalized;
        Vector2 pathDirection = ((Vector2)path.vectorPath[currentWayPoint] - rb.position).normalized;

        float distance = Vector2.Distance(rb.position, path.vectorPath[currentWayPoint]);    
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        angle *= Mathf.Sign(transform.localScale.x);
        pivot.transform.localRotation = Quaternion.AngleAxis(angle, Vector3.forward);

        rb.velocity = pathDirection * speed;

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
            transform.localScale = new Vector3(Mathf.Lerp(-direction, direction, time / flipSpeed), 1, 1);
            time += Time.deltaTime;
            yield return null;
        }

        transform.localScale = new Vector3(direction, 1, 1);
    }
}
