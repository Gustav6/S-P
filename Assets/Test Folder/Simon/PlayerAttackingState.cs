using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAttackingState : PlayerBaseState
{
    private CircleCollider2D _hitbox;
    private Vector2 _weaponOffset;
    private Transform _playerTransform;

    internal override void EnterState(PlayerControllerAttackTest player)
    {
        _hitbox = player.CurrentWeapon.Hitbox;
        _weaponOffset = player.CurrentWeapon.HitboxToPlayerOffset;
        _playerTransform = player.transform;

        player.StartCoroutine(StartAttack(player));
    }

    /// <summary>
    /// Creates an attacking hitbox.
    /// </summary>
    internal IEnumerator StartAttack(PlayerControllerAttackTest player)
    {
        // TODO: Create animation presets depending on weapon type.
        // Use the indvidual weapons speeds to determine animation speeds.
        // Rotate where the animation plays depending on where the player is looking.

        yield return new WaitForSeconds(player.CurrentWeapon.StartUp);

        var temp = Object.Instantiate(_hitbox, new Vector2(_playerTransform.position.x + _weaponOffset.x,
                    _playerTransform.position.y + _weaponOffset.y), Quaternion.Euler(0, 0, 0), _playerTransform);

        yield return new WaitForSeconds(player.CurrentWeapon.Active);

        Object.Destroy(temp.gameObject);

        yield return new WaitForSeconds(player.CurrentWeapon.WindDown);

        player.SwitchState(player.defaultState);
    }

    internal override void UpdateState(PlayerControllerAttackTest player)
    {

    }

    internal override void ExitState(PlayerControllerAttackTest player)
    {

    }
}
