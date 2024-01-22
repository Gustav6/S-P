using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestEnemyAI : MonoBehaviour
{
    [SerializeField] float _movementSpeed;

    [SerializeField] Transform _playerTransform;

    Rigidbody2D _rb;

    private void Awake()
    {
        _rb = GetComponent<Rigidbody2D>();
    }

    private void Update()
    {
        Vector2 direction;

        if (!CanSeeTarget(transform.position, _playerTransform.position))
        {
            Vector2 targetPos = GetComponent<Pathfinder>().FindPath(TilemapManager.Instance.Grid[TilemapManager.Instance.WorldToGridPos(transform.position)],
                                TilemapManager.Instance.Grid[TilemapManager.Instance.WorldToGridPos(_playerTransform.position)],
                                _playerTransform.position);

            while (!CanSeeTarget(transform.position, targetPos))
            {
                targetPos = GetComponent<Pathfinder>().FindPath(TilemapManager.Instance.Grid[TilemapManager.Instance.WorldToGridPos(transform.position)],
                                TilemapManager.Instance.Grid[TilemapManager.Instance.WorldToGridPos(_playerTransform.position)],
                                _playerTransform.position);
            }

            direction = (targetPos - (Vector2)_playerTransform.position);
        }
        else
            direction = (_playerTransform.position - transform.position).normalized;

        _rb.velocity = direction * _movementSpeed;
    }

    bool CanSeeTarget(Vector2 currentPos, Vector2 targetPos)
    {
        Vector2 targetDirection = targetPos - currentPos;

        Ray ray = new(currentPos, targetDirection);

        for (int i = 0; i < 5; i++)
        {
            if (!TilemapManager.Instance.HasWalkableTile(TilemapManager.Instance.WorldToGridPos(ray.GetPoint(2 / 5 * i))))
                return false;
        }

        return true;
    }
}
