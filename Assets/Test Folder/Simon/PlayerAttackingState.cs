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

        player.StartCoroutine(Attack(player));
    }

    /// <summary>
    /// Creates an attacking hitbox.
    /// </summary>
    internal IEnumerator Attack(PlayerControllerAttackTest player)
    {
        // TODO: Clean-up and animation

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
