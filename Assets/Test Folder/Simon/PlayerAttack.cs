using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAttack : MonoBehaviour
{
    [SerializeField] Transform _directionIndicator;

    private PlayerAnimationController _attackController;
    private AimController _aimController;
    private PlayerStats _player;

    private void Awake()
    {
        _attackController = GetComponentInChildren<PlayerAnimationController>();
        _aimController = GetComponent<AimController>();
        _player = GetComponent<PlayerStats>();
    }

    private void Update()
    {
        if (!_attackController.inAnimation)
            TurnToMouse();

        if (Input.GetMouseButton(0) && !_attackController.inAnimation && EquipmentManager.Instance.CanHit())
        {
            Vector2 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            Vector2 targetDirection = (mousePosition - (Vector2)transform.position).normalized;

            _attackController.PlayHitAnimation(targetDirection);
        }

        if (Input.GetKeyDown(KeyCode.Space) && _player.currentPowerUp != null)
            _player.currentPowerUp.UsePowerUp();
    }

    private void TurnToMouse()
    {
        Vector2 targetPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);

        Vector2 targetDirection = (targetPosition - (Vector2)transform.position).normalized;
        // If this is giving you an error, copy the DirectionIndicator object in JacobScene under Entities/Player and assign it to this script through the inspector.
        _directionIndicator.eulerAngles = new Vector3(0, 0, Mathf.Atan2(targetDirection.y, targetDirection.x) * Mathf.Rad2Deg - 90); 

        _aimController.FaceTarget(targetPosition);
    }
}
