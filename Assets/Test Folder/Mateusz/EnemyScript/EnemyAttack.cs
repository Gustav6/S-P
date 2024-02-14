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

    public bool CanEnemyAttack { get; private set; }

    [SerializeField] internal GameObject hitbox, hitboxSpawn;

    private void Awake()
    {
        _enemy = GetComponent<Enemy>();

        if (_enemy.waveMachine.CurrentState != _enemy.waveMachine.States[WaveStateMachine.WaveState.WaveLoss])
            CanAttack(true);
        else
        {
            CanAttack(false);
        }
    }

    private void Start()
    {
        _enemyAttackController = _enemy._attackController;
        _player = PlayerStats.Instance.transform;
    }

    public void CanAttack(bool shouldEnemyAttack)
    {
        CanEnemyAttack = shouldEnemyAttack;
    }

    public bool DistanceToTarget()
    {
        float dist = Vector2.Distance(transform.position, _player.position);

        return dist <= minDist;
    }

    void Update()
    {
        if (!CanEnemyAttack)
            return;

        if (isAttacking && attackReady)
        {
            isAttacking = false;
        }

        if (!DistanceToTarget())
            _enemyAttackController.EnterMovement();
        else
        {
            _enemyAttackController.LeaveMovement(true);
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
            attackReady = false;
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
        _enemy._attackController.LeaveMovement(true);
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
