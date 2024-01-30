using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerControllerAttackTest : MonoBehaviour, IDamageable
{
    public float KnockbackPercent { get; set; }

    private AttackController _playerAttackController;

    private void Awake()
    {
        _playerAttackController = GetComponentInChildren<AttackController>();
    }

    public void TakeKnockback(float knockbackMultiplier)
    {
        Debug.Log($"Current player percent: {KnockbackPercent}%\nPlayer knockback as: {KnockbackPercent * knockbackMultiplier}");
    }

    private void Update()
    {
        if (Input.GetKey(KeyCode.Space) && !_playerAttackController.IsAnimationPlaying)
        {
            _playerAttackController.PlayHitAnimation();
        }

        TurnToMouse();
    }

    /// <summary>
    /// For testing, remove later.
    /// </summary>
    private void TurnToMouse()
    {
        Vector3 mousePosition = Input.mousePosition;
        Vector3 targetPosition = Camera.main.ScreenToWorldPoint(mousePosition);

        Vector3 direction = targetPosition - _playerAttackController._weaponRotationPoint.position;
        float angle = Mathf.Atan2(direction.x, direction.y) * -Mathf.Rad2Deg;
        _playerAttackController._weaponRotationPoint.rotation = Quaternion.AngleAxis(angle, Vector3.forward);
    }
}
