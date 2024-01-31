using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerControllerAttackTest : MonoBehaviour, IDamageable
{
    public float KnockbackPercent { get; set; }

    private AttackController _attackController;

    // Remove later, maybe.
    internal Transform _weaponRotationPoint;

    private void Awake()
    {
        _attackController = GetComponentInChildren<AttackController>();
        _weaponRotationPoint = transform.GetChild(0);
    }

    public void TakeKnockback(float knockbackMultiplier)
    {
        Debug.Log($"Current player percent: {KnockbackPercent}%\nPlayer knockback as: {KnockbackPercent * knockbackMultiplier}");
    }

    private void Update()
    {
        if (Input.GetKey(KeyCode.Space) && !_attackController.IsAnimationPlaying)
        {
            Vector2 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            Vector2 targetDirection = (mousePosition - (Vector2)transform.position).normalized;

            _attackController.PlayHitAnimation(targetDirection);
        }

        TurnToMouse();
    }

    /// <summary>
    /// For testing, remove later.
    /// </summary>
    private void TurnToMouse()
    {
        Vector2 targetPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);

        Vector2 direction = targetPosition - (Vector2)_weaponRotationPoint.position;
        float angle = Mathf.Atan2(direction.x, direction.y) * -Mathf.Rad2Deg;
        _weaponRotationPoint.rotation = Quaternion.AngleAxis(angle, Vector3.forward);
    }
}
