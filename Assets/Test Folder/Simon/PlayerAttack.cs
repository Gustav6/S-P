using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Users;
using UnityEngine.UI;

public class PlayerAttack : MonoBehaviour
{
    [SerializeField] Transform _directionIndicator;

    [SerializeField] InputAction _attackIA;
    [SerializeField] InputAction _powerupIA;

    [SerializeField] Image _kbInputImage, _gamepadInputImage;
    [SerializeField] Sprite[] _buttonSprites;

    private PlayerAnimationController _attackController;
    private AimController _aimController;
    private PlayerStats _player;

    bool _attackInputHeld;
    bool _powerupInputHeld;

    int _buttonIndex;

    private void Awake()
    {
        _attackController = GetComponentInChildren<PlayerAnimationController>();
        _aimController = GetComponent<AimController>();
        _player = GetComponent<PlayerStats>();

        _attackIA.performed += ctx => { OnHit(ctx); };
        _powerupIA.performed += ctx => { OnPowerUpUsed(ctx); };
    }

    private void Update()
    {
        if (!_attackController.inAnimation)
            TurnToMouse();

        if (_attackInputHeld && !_attackController.inAnimation && EquipmentManager.Instance.CanHit())
        {
            Vector2 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            Vector2 targetDirection = (mousePosition - (Vector2)transform.position).normalized;

            _attackController.PlayHitAnimation(targetDirection);
        }

        if (_powerupInputHeld && _player.currentPowerUp != null && !_player.PowerupActive)
            _player.currentPowerUp.UsePowerUp();

        if (_buttonIndex == 0)
        {
            if (_powerupInputHeld && _kbInputImage.sprite != _buttonSprites[_buttonIndex])
                _kbInputImage.sprite = _buttonSprites[_buttonIndex];
            else if (!_powerupInputHeld && _kbInputImage.sprite != _buttonSprites[_buttonIndex + 1])
                _kbInputImage.sprite = _buttonSprites[_buttonIndex + 1];
        }
        else
        {
            if (_powerupInputHeld && _gamepadInputImage.sprite != _buttonSprites[_buttonIndex])
                _gamepadInputImage.sprite = _buttonSprites[_buttonIndex];
            else if (!_powerupInputHeld && _gamepadInputImage.sprite != _buttonSprites[_buttonIndex + 1])
                _gamepadInputImage.sprite = _buttonSprites[_buttonIndex + 1];
        }
    }

    void OnPowerUpUsed(InputAction.CallbackContext ctx)
    {
        _powerupInputHeld = ctx.ReadValueAsButton();

        if (ctx.control.device is Keyboard)
        {
            _kbInputImage.gameObject.SetActive(true);
            _gamepadInputImage.gameObject.SetActive(false);
            _buttonIndex = 0;
        }
        else if (ctx.control.device is Gamepad)
        {
            _gamepadInputImage.gameObject.SetActive(true);
            _kbInputImage.gameObject.SetActive(false);
            _buttonIndex = 2;
        }
    }

    void OnHit(InputAction.CallbackContext ctx)
    {
        _attackInputHeld = ctx.ReadValueAsButton();
    }

    private void TurnToMouse()
    {
        Vector2 targetPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);

        Vector2 targetDirection = (targetPosition - (Vector2)transform.position).normalized;
        // If this is giving you an error, copy the DirectionIndicator object in JacobScene under Entities/Player and assign it to this script through the inspector.
        _directionIndicator.eulerAngles = new Vector3(0, 0, Mathf.Atan2(targetDirection.y, targetDirection.x) * Mathf.Rad2Deg - 90); 

        _aimController.FaceTarget(targetPosition);
    }
    public void OnEnable()
    {
        _attackIA.Enable();
        _powerupIA.Enable();
    }

    public void OnDisable()
    {
        _attackIA.Disable();
        _powerupIA.Enable();
    }
}
