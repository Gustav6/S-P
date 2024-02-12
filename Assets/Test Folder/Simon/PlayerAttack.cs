using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class PlayerAttack : MonoBehaviour
{
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
        if (EquipmentManager.Instance.CanHit())
            TurnToMouse();

        if (Input.GetKey(KeyCode.Q) && EquipmentManager.Instance.CanHit())
        {
            Vector2 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            Vector2 targetDirection = (mousePosition - (Vector2)transform.position).normalized;

            _attackController.PlayHitAnimation(targetDirection);
        }

        if (Input.GetKeyDown(KeyCode.F) && _player.currentPowerUp != null)
            _player.currentPowerUp.UsePowerUp();

        if (Input.GetKeyDown(KeyCode.K))
            EquipmentManager.Instance.OnSpawnPowerUp?.Invoke(Vector2.zero, 100, PowerUpTypes.Dash);
    }

    private void TurnToMouse()
    {
        Vector2 targetPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);

        _aimController.FaceTarget(targetPosition);
    }
}
