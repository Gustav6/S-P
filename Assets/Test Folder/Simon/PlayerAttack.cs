using System;
using System.Collections;
using System.Collections.Generic;
using System.Xml.Serialization;
using UnityEditor;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Users;
using UnityEngine.UI;

// Jacob & Simon
public class PlayerAttack : MonoBehaviour
{
    [SerializeField] Transform _directionIndicator;

    [SerializeField] InputAction _attackIA;
    [SerializeField] InputAction _powerupIA;
    [SerializeField] InputAction _anyIA;
    [SerializeField] InputAction _aimIA;

    [SerializeField] Image _kbInputImage, _gamepadInputImage;
    [SerializeField] Sprite[] _buttonSprites;

    [SerializeField] private PauseController _pauseController;

    private PlayerAnimationController _attackController;
    private AimController _aimController;
    private PlayerStats _player;

    bool _attackInputHeld;
    bool _powerupInputHeld;
    bool _paused;

    bool _isOnKBM = true;

    private void Awake()
    {
        _attackController = GetComponentInChildren<PlayerAnimationController>();
        _aimController = GetComponent<AimController>();
        _player = GetComponent<PlayerStats>();

        _attackIA.performed += ctx => { OnHit(ctx); };
        _powerupIA.performed += ctx => { OnPowerUpUsed(ctx); };
        _anyIA.performed += ctx => { OnAnyInput(ctx); };
    }

    void OnAnyInput(InputAction.CallbackContext ctx)
    {
        if (ctx.control.device is Keyboard || ctx.control.device is Mouse)
        {
            _isOnKBM = true;
            if (!_kbInputImage.gameObject.activeSelf)
            {
                _kbInputImage.gameObject.SetActive(true);
                _gamepadInputImage.gameObject.SetActive(false);
            }
        }
        else if (ctx.control.device is Gamepad)
        {
            _isOnKBM = false;
            if (!_gamepadInputImage.gameObject.activeSelf)
            {
                _gamepadInputImage.gameObject.SetActive(true);
                _kbInputImage.gameObject.SetActive(false);
            }
        }
    }

    private void TogglePlayerAction(object sender, EventArgs args)
    {
        _paused = ((OnPausedEventArgs)args).isPaused;
    }

    private void Update()
    {
        if (_isOnKBM)
            TurnToTarget(Camera.main.ScreenToWorldPoint(Input.mousePosition));
        else
        {
            Debug.Log(_aimIA.ReadValue<Vector2>());
            TurnToTargetController(_aimIA.ReadValue<Vector2>().normalized);
        }

        if (_attackInputHeld && !_attackController.inAnimation && EquipmentManager.Instance.CanHit())
        {
            Vector2 targetDirection;

            if (_isOnKBM)
                targetDirection = GetMouseDirection();
            else
                targetDirection = _aimIA.ReadValue<Vector2>().normalized;

            _attackController.PlayHitAnimation(targetDirection);
        }

        if (_powerupInputHeld && _player.currentPowerUp != null && !_player.PowerupActive)
            _player.currentPowerUp.UsePowerUp();

        if (_isOnKBM)
        {
            if (_powerupInputHeld && _kbInputImage.sprite != _buttonSprites[0])
                _kbInputImage.sprite = _buttonSprites[0];
            else if (!_powerupInputHeld && _kbInputImage.sprite != _buttonSprites[1])
                _kbInputImage.sprite = _buttonSprites[1];
        }
        else
        {
            if (_powerupInputHeld && _gamepadInputImage.sprite != _buttonSprites[2])
                _gamepadInputImage.sprite = _buttonSprites[2];
            else if (!_powerupInputHeld && _gamepadInputImage.sprite != _buttonSprites[3])
                _gamepadInputImage.sprite = _buttonSprites[3];
        }
    }

    // Simon
    private Vector2 GetMouseDirection()
    {
        Vector2 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        Vector2 targetDirection = (mousePosition - (Vector2)transform.position).normalized;

        return targetDirection;
    }

    void OnPowerUpUsed(InputAction.CallbackContext ctx)
    {
        _powerupInputHeld = ctx.ReadValueAsButton();
    }

    void OnHit(InputAction.CallbackContext ctx)
    {
        _attackInputHeld = ctx.ReadValueAsButton();
    }

    // Simon
    private void TurnToTarget(Vector2 targetPosition)
    {
        Vector2 targetDirection = (targetPosition - (Vector2)transform.position).normalized;
        // If this is giving you an error, copy the DirectionIndicator object in JacobScene under Entities/Player and assign it to this script through the inspector.
        _directionIndicator.eulerAngles = new Vector3(0, 0, Mathf.Atan2(targetDirection.y, targetDirection.x) * Mathf.Rad2Deg - 90); 

        _aimController.FaceTarget(targetPosition);
    }

    // Simon
    private void TurnToTargetController(Vector2 targetDirection)
    {
        // If this is giving you an error, copy the DirectionIndicator object in JacobScene under Entities/Player and assign it to this script through the inspector.
        _directionIndicator.eulerAngles = new Vector3(0, 0, Mathf.Atan2(targetDirection.y, targetDirection.x) * Mathf.Rad2Deg - 90);

        _aimController.FaceDirection(targetDirection);
    }

    public void OnEnable()
    {
        _attackIA.Enable();
        _powerupIA.Enable();
        _anyIA.Enable();
    }

    public void OnDisable()
    {
        _attackIA.Disable();
        _powerupIA.Disable();
        _anyIA.Disable();
    }
}
