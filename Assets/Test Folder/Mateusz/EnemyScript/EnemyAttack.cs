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

    private GameObject hitbox;

    private void Awake()
    {
        enemyAttackController = GetComponentInChildren<EnemyAttackController>();
        anim = GetComponentInChildren<Animator>();

        player = FindFirstObjectByType<PlayerMovement>();
        enemy = FindFirstObjectByType<Enemy>();
    }

   private void spawnEnemyHitbox()
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

        float dist = Vector2.Distance(transform.localPosition, player.transform.localPosition);

        if(dist < minDist)
        {
            enemyAttackController.EnterAttackState();
            Debug.Log("Attacking");
        }
        else if (dist > minDist)
        {
            enemyAttackController.LeaveAttack();
            Debug.Log("Cannot attack");
        }
   

        Debug.Log("Distance from player: " +dist);
    }
}

