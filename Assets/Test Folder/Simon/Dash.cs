using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Dash : PowerUp
{
    private PlayerMovement _player;
    private Rigidbody2D _rb;

    private BoxCollider2D _playerHitbox;

    private float _dashTime = 0.5f;

    private void Awake()
    {
        _player = GetComponent<PlayerMovement>();
        _rb = GetComponent<Rigidbody2D>();

        _playerHitbox = GetComponent<BoxCollider2D>();
    }

    public override void UsePowerUp()
    {
        Vector2 direction = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical")).normalized;

        // TODO: Add a little animation on the hud or something when the button is pressed to show that the player is trying to use powerup.
        if (direction == Vector2.zero)
            return;

        StartCoroutine(AttackLogic.AddBoost(_player, _rb, direction, 20, _dashTime));

        StartCoroutine(MakePlayerInvincible());
    }

    // TODO: Add a little something to show invincibility.
    private IEnumerator MakePlayerInvincible()
    {
        _playerHitbox.enabled = false;

        yield return new WaitForSeconds(_dashTime);

        _playerHitbox.enabled = true;
        PlayerStats.Instance.ClearEquippedAbility();
    }
}
