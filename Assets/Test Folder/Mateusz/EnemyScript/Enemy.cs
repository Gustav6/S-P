
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class Enemy : MonoBehaviour, IDamageable
{
    public float Damage, KnockbackMultiplier;

    [HideInInspector]
    public Rigidbody2D _rb;

    [HideInInspector]
    public EnemyAI _enemyAI;

    [HideInInspector]
    public EnemyAttackController _attackController;

    [HideInInspector]
    public EnemyAttack _enemyAttack;

    private IDamageable _thisDamagable;
    private Tilemap _tilemap;
    private Dictionary<Vector2Int, TileBase> _tiles;
    private BoxCollider2D _thisFeetCollider;

    public float KnockbackPercent { get; set; }

    private void Awake()
    {
        _rb = GetComponent<Rigidbody2D>();
        _enemyAI = GetComponent<EnemyAI>();
        _attackController = GetComponent<EnemyAttackController>();
        _enemyAttack = GetComponent<EnemyAttack>();

        _thisDamagable = this;

        _thisFeetCollider = GetComponentsInChildren<BoxCollider2D>()[1];
    }

    private void Start()
    {
        _tilemap = PlayerStats.Instance.tilemap;
        _tiles = IDamageable.PopulateTilesDictonary(_tilemap);
    }

    public virtual void TakeKnockback(Vector2 sourcePosition, float knockbackMultiplier, float stunDuration)
    {
        StartCoroutine(GiveEnemyMovement(stunDuration));
    }

    internal IEnumerator GiveEnemyMovement(float time)
    {
        yield return new WaitForSeconds(time);
        _attackController.EnterMovement();
        _enemyAttack.CanAttack(true);

        _thisDamagable.CheckDeath(_tilemap, _tiles, _thisFeetCollider);
    }

    private IEnumerable<Vector2> GetEnemyEnemyVelocity(Vector2 knockbackVector, float multiplier, float stun)
    {
        Vector2 returningVector;

        float t = 0;

        while (t <= stun)
        {
            returningVector = (knockbackVector * multiplier) - ((knockbackVector * multiplier) * (t / stun));

            yield return returningVector;

            t += Time.deltaTime;
        }
    }

    internal IEnumerator SetEnemyVelocity(Vector2 knockbackVector, float multiplier, float stunDuration)
    {
        foreach (Vector2 velocity in GetEnemyEnemyVelocity(knockbackVector, multiplier, stunDuration))
        {
            _rb.velocity = velocity;
            yield return null;
        }
    }

    public void Die()
    {
        if (EquipmentManager.Instance.CanSpawnPowerUps)
        {
            EquipmentManager.Instance.SetPowerUpCanSpawn(false);
            EquipmentManager.Instance.OnSpawnPowerUp(transform.position, 100, PowerUpTypes.Anything);
        }

        Destroy(gameObject);
    }
}
