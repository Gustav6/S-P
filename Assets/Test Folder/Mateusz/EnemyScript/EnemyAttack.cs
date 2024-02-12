using NUnit.Framework;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAttack : MonoBehaviour
{
    private EnemyAttackController _enemyAttackController;
    Enemy _enemy;

    Transform _player;

    [SerializeField] float minDist = 1.5f;

    float attackCooldown;
    [SerializeField] float maxCooldown = 2.5f;

    [SerializeField] private bool hasAttacked;
    [SerializeField] private bool attackReady;
   [SerializeField] private bool isAttacking;

    private bool canAttack = true;

    [SerializeField] internal GameObject hitbox, hitboxSpawn;

    private void Awake()
    {
        _enemy = GetComponent<Enemy>();
    }

    private void Start()
    {
        _enemyAttackController = _enemy._attackController;
        _player = PlayerStats.Instance.transform;
    }

    public void CanAttack(bool shouldEnemyAttack)
    {
        canAttack = shouldEnemyAttack;
    }

    public bool DistanceToTarget()
    {
        float dist = Vector2.Distance(transform.position, _player.position);

        return dist <= minDist;
    }

    void Update()
    {
        if (!canAttack)
            return;

        if (!DistanceToTarget())
            _enemyAttackController.EnterMovement();
        else
        {
            _enemyAttackController.LeaveMovement();
        }

        if (!isAttacking && attackReady)
            EnemyAttacking();

        if (hasAttacked && attackReady)
        {
            attackCooldown = 0;
            hasAttacked = false;
            attackReady = false;
        }

        if (attackCooldown >= maxCooldown)
        {
            attackReady = true;
        }
        else
        {
            attackCooldown += Time.deltaTime;
        }
    }

    public void EnemyAttacking()
    {
        if (DistanceToTarget())
        {
            IsAttacking();
        }
    }

    void IsAttacking()
    {
        attackReady = false;
        _enemyAttackController.EnterAttackState();
        _enemy._attackController.LeaveMovement();
        isAttacking = true;
    }

    public void EnemyStopAttacking()
    {
        hasAttacked = true;
        isAttacking = false;
        _enemy._attackController.EnterMovement();
        _enemy._attackController.LeaveAttack();
    }
}
