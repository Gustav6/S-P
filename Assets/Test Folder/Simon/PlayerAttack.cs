using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
        {
            EquipmentManager.Instance.OnSpawnPowerUp?.Invoke(Vector2.zero, 100, PowerUpTypes.Dash);

            EquipmentManager.Instance.OnSpawnPowerUp?.Invoke(Vector2.right * 2, 100, PowerUpTypes.Haste);

            EquipmentManager.Instance.OnSpawnPowerUp?.Invoke(Vector2.left * 2, 100, PowerUpTypes.Tank);
        }

        if (Input.GetKeyDown(KeyCode.J))
            EquipmentManager.Instance.OnSpawnPowerUp?.Invoke(Vector2.up * 2, 100, PowerUpTypes.Anything);
    }

    private void TurnToMouse()
    {
        Vector2 targetPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);

        _aimController.FaceTarget(targetPosition);
    }
}
