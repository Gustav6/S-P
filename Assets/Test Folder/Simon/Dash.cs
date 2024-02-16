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
    private SpriteRenderer _bodySr;

    private readonly float _dashTime = 0.5f;

    private void Awake()
    {
        _player = GetComponent<PlayerMovement>();
        _rb = GetComponent<Rigidbody2D>();
        _playerHitbox = GetComponent<BoxCollider2D>();

        _sr = GetComponentsInChildren<SpriteRenderer>().ToList();
        _sr.RemoveAt(4); // 4 = Weapon flash sprite.
        _bodySr = _sr[0];
    }

    private void Start()
    {
        powerUpSprite = EquipmentManager.Instance.ReturnPowerupSprite(PowerUpTypes.Dash);

        EquipmentManager.Instance.OnPowerUpEquipped?.Invoke();
    }

    public override void UsePowerUp()
    {
        EquipmentManager.Instance.OnPowerUpUsed?.Invoke();

        Vector2 direction = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical")).normalized;

        if (direction == Vector2.zero)
            return;

        StartCoroutine(AttackLogic.AddBoost(_player, _rb, direction, 20, _dashTime));

        StartCoroutine(MakePlayerInvincible());
    }

    private IEnumerator MakePlayerInvincible()
    {
        _playerHitbox.enabled = false;

        Sprite startSprite = _bodySr.sprite;

        GameObject hitbox = Instantiate(PlayerStats.Instance.CurrentWeapon.Hitbox, transform);
        hitbox.transform.localPosition = Vector2.zero;

        foreach (SpriteRenderer sr in _sr)
        {
            sr.enabled = false;
        }

        _bodySr.enabled = true;
        _bodySr.sprite = PlayerStats.Instance.dashSprite;

        yield return new WaitForSeconds(_dashTime);

        foreach (SpriteRenderer sr in _sr)
        {
            sr.enabled = true;
        }

        _bodySr.sprite = startSprite;

        Destroy(hitbox);
        OnDeactivatePowerUp();
    }

    public override void OnDeactivatePowerUp()
    {
        _playerHitbox.enabled = true;
        PlayerStats.Instance.ClearEquippedAbility();
    }
}
