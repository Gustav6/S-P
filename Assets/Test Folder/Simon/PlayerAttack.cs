using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAttack : MonoBehaviour
{
    private AttackController _attackController;
    private AimController _aimController;

    private void Awake()
    {
        _attackController = GetComponentInChildren<AttackController>();
        _aimController = GetComponent<AimController>();
    }

    private void Update()
    {
        if (Input.GetKey(KeyCode.Q) && !_attackController.IsAnimationPlaying)
        {
            Vector2 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            Vector2 targetDirection = (mousePosition - (Vector2)transform.position).normalized;

            _attackController.PlayHitAnimation(targetDirection);
        }

        if (!_attackController.IsAnimationPlaying)
            TurnToMouse();
    }

    private void TurnToMouse()
    {
        Vector2 targetPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);

        _aimController.FaceTarget(targetPosition);
    }
}
