using NUnit.Framework;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAttack : MonoBehaviour
{
    private EnemyAttackController _enemyAttackController;

    PlayerStats _player;
    Enemy _enemy;

    Animator _anim;

    public Transform _target;

    float minDist = 3f;

    float attackCooldown;
    float maxCooldown = 3f;

    private bool hasAttacked;
    bool attackReady;
    private bool isAttacking;

    [SerializeField] internal float damage, knockbackMultiplier;

    [SerializeField] private GameObject hitbox;



    private void Awake()
    {
        _enemyAttackController = GetComponentInChildren<EnemyAttackController>();
        _anim = GetComponentInChildren<Animator>();
        _player = FindFirstObjectByType<PlayerStats>();
        _enemy = FindFirstObjectByType<Enemy>();

    }

    private void spawnEnemyHitbox()
    {

    }

    void Update()
    {
        EnemyAttacking();

        if (hasAttacked && attackReady)
        {
            attackCooldown = 0;
            hasAttacked = false;
        }

        if (attackCooldown >= maxCooldown)
        {
            attackReady = true;
        }
        else
        {
            attackCooldown += Time.deltaTime;
            attackReady = false;
        }
    }

    public void EnemyAttacking()
    {
        float dist = Vector2.Distance(transform.position, _player.transform.position);

        if (dist <= minDist && attackReady && !isAttacking)
        {
            StartCoroutine(IsAttacking());
        }
        else if (!attackReady)
        {
            _enemyAttackController.LeaveAttack();
            hitbox.SetActive(false);
        }
    }

    IEnumerator IsAttacking()
    {
        _enemyAttackController.EnterAttackState();
        isAttacking = true;
        hitbox.SetActive(true);

        yield return new WaitForSeconds(3);

        hasAttacked = true;
        isAttacking = false;
    }
}

