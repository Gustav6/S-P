using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShrimpEnemy : Enemy, IDamageable
{
    public float KnockbackPercent { get; set; }

    Enemy _enemy;
    EnemyAI _enemyAI;

    private void Start()
    {
         _enemy = FindFirstObjectByType< Enemy>();
         _player = FindFirstObjectByType<PlayerMovement>();
         _rb = GetComponent<Rigidbody2D>();

        _enemyAttack = GetComponentInChildren<EnemyAttack>();
        _enemyAI = GetComponent<EnemyAI>();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.K))
        {
            TakeKnockback(Vector2.zero, 2, 0.25f);
        }
    }

    public void TakeKnockback(Vector2 sourcePosition, float knockbackMultiplier, float stuntDuration)
    {
        Vector2 knockbackVector = ((Vector2)transform.position - sourcePosition).normalized;

        _enemyAI.CanMove = false;

        _rb.AddForce(knockbackVector * knockbackMultiplier, ForceMode2D.Impulse);

        StartCoroutine(GiveEnemyMovement(stuntDuration));
    }

    IEnumerator GiveEnemyMovement(float time)
    {
        yield return new WaitForSeconds(time);
        _enemyAI.CanMove = true;
    }
}
