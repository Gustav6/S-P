using NUnit.Framework;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAttack : MonoBehaviour
{
    private EnemyAttackController _enemyAttackController;
    Enemy _enemy;

    Transform _player;

    float minDist = 1.5f;

    float attackCooldown;
    float maxCooldown = 4f;

    private bool hasAttacked;
    private bool attackReady;
    private bool isAttacking;

    [SerializeField] private GameObject hitbox;

    private void Awake()
    {
        _enemy = GetComponent<Enemy>();
    }

    private void Start()
    {
        _enemyAttackController = _enemy._attackController;
        _player = PlayerStats.Instance.transform;
    }

    void Update()
    {
        if (DistanceToTarget())
            _enemyAttackController.LeaveMovement();
        else
        {
            _enemyAttackController.EnterMovement();
        }

        if (!isAttacking)
        {
            _enemyAttackController.LeaveAttack();
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
            StartCoroutine(IsAttacking());
        }
    }

    public bool DistanceToTarget()
    {
        float dist = Vector2.Distance(transform.position, _player.position);

        return dist <= minDist;
    }

    IEnumerator IsAttacking()
    {
        _enemyAttackController.EnterAttackState();
        _enemy._enemyAI.CanMove = false;
        isAttacking = true;
        hitbox.SetActive(true);

        // TODO: Speed of actual animation + a little.
        yield return new WaitForSeconds(0.6f);

        hitbox.SetActive(false);
        hasAttacked = true;
        isAttacking = false;
        // TODO: Change depending on enemies.
        _enemy._enemyAI.CanMove = true;
    }
}
