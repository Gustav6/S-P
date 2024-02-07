using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAttack : MonoBehaviour
{
    private EnemyAttackController enemyAttackController;

    PlayerMovement player;
    Enemy enemy;

    Animator anim;

    public Transform target;

    float minDist = 3f;
    bool attackReady;

    float attackCooldown;
    float maxCooldown = 3f;

    private void Awake()
    {
        enemyAttackController = GetComponentInChildren<EnemyAttackController>();
        anim = GetComponentInChildren<Animator>();

        player = FindFirstObjectByType<PlayerMovement>();
        enemy = FindFirstObjectByType<Enemy>();
    }

    private void Start()
    {
        
    }

    void Update()
    {
        attackCooldown += Time.deltaTime;

        if (attackCooldown >= maxCooldown)
        {
            attackReady = true;
        }
        else
        {
            attackReady = false;
        }

        float dist = Vector3.Distance(transform.localPosition, player.transform.localPosition);

        if(dist <= minDist)
        {
            anim.SetTrigger("Attack ");
            Debug.Log("Attacking");
        }
        else
        {
            Debug.Log("Cannot attack");
        }

        Debug.Log("Distance from player: " +dist);
    }
}

