using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Dash : PowerUp
{
    private PlayerMovement _player;
    private Rigidbody2D _rb;
    private BoxCollider2D _playerHitbox;

    private List<SpriteRenderer> _sr;
    private Color[] _initialColor;

    private Color _invincibleColor = new Color(0.6f, 0.7f, 0.7f);

    private float _dashTime = 0.5f;

    private void Awake()
    {
        _player = GetComponent<PlayerMovement>();
        _rb = GetComponent<Rigidbody2D>();
        _playerHitbox = GetComponent<BoxCollider2D>();

        _sr = GetComponentsInChildren<SpriteRenderer>().ToList();
        _sr.RemoveAt(4); // 4 = Weapon flash sprite.

        _initialColor = new Color[_sr.Count];
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

    private IEnumerator MakePlayerInvincible()
    {
        _playerHitbox.enabled = false;

        int rendererIndex = 0;

        foreach (SpriteRenderer sr in _sr)
        {
            _initialColor[rendererIndex] = sr.color;
            rendererIndex++;

            sr.color = _invincibleColor;
        }

        yield return new WaitForSeconds(_dashTime);

        for (int i = 0; i < rendererIndex; i++)
        {
            _sr[i].color = _initialColor[i];
        }

        OnDeactivatePowerUp();
    }

    public override void OnDeactivatePowerUp()
    {
        _playerHitbox.enabled = true;
        PlayerStats.Instance.ClearEquippedAbility();
    }
}
