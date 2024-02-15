using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
using TMPro;

public class PlayerStats : MonoBehaviour, IDamageable
{
    #region Singleton

    public static PlayerStats Instance;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Debug.LogError("More than one instance of PlayerStats found on " + gameObject + ", Destroying instance");
            Destroy(this);
        }

        _playerMovement = GetComponent<PlayerMovement>();
        _allSrsOnPlayer = GetComponentsInChildren<SpriteRenderer>();
        _headRenderer = _allSrsOnPlayer[1];

        _playerCollider = GetComponent<Collider2D>();
        _thisDamagable = this;

        _initialHead = _headRenderer.sprite;
        _initialBody = _allSrsOnPlayer[0].sprite;
    }

    #endregion

    #region Saved Data
    private PlayerData _data;
    // Remove the use of constructor later.
    StatBlock _mainStatBlock;
    public WeaponSO CurrentWeapon; // Set to starting weapon.
    #endregion

    StatBlock _weaponStatBlock = new(1, 1, 1, 1, 1, 1);
    StatBlock _abilityStatBlock = new(1, 1, 1, 1, 1, 1);

    private PlayerMovement _playerMovement;

    [SerializeField] internal WaveStateMachine waveStateMachine;
    [SerializeField] private GameObject _gameOverGameObject;

    private SpriteRenderer _headRenderer;
    private SpriteRenderer[] _allSrsOnPlayer;
    private Sprite _initialHead, _initialBody;
    [SerializeField] private Sprite _angryHead, _playerKnockbackSprite;
    [SerializeField] internal Sprite dashSprite;

    [SerializeField] internal Tilemap tilemap;
    private Dictionary<Vector2Int, TileBase> _tiles;
    private Collider2D _playerCollider;
    private IDamageable _thisDamagable;

    [SerializeField] float _diStrength = 0.25f; // DI stands for direction input, used to reduce or enhance knockback when counteracting it with movement input.
    [SerializeField] TMP_Text _damageDisplayText;
    [SerializeField] Gradient _damageGradient;
    float _currentDamageDisplay;
    float _desiredDamageDisplay;

    bool _isImmune = false;

    [SerializeField] float _maxDamagePercent = 300f;
    public float KnockbackPercent { get; set; }

    public PowerUp currentPowerUp { get; private set; }

    public int Score;

    private void Start()
    {
        _data = SaveSystem.Instance.LoadGameSave();
        SetLocalDataToSave(_data);

        _tiles = IDamageable.PopulateTilesDictonary(tilemap);

        // Call this when you want to change the player weapon n stuff.
        EquipmentManager.Instance.SwitchWeapon(CurrentWeapon);
    }

    private void Update()
    {
        _thisDamagable.CheckDeath(tilemap, _tiles, _playerCollider);

        _currentDamageDisplay = Mathf.Lerp(_currentDamageDisplay, _desiredDamageDisplay, Time.deltaTime * 10);
        _damageDisplayText.text = ((int)_currentDamageDisplay).ToString() + "%";
        _damageDisplayText.color = _damageGradient.Evaluate(_currentDamageDisplay / _maxDamagePercent);

        if (_currentDamageDisplay >= 299 && KnockbackPercent == 300)
            _damageDisplayText.text = "300%";
    }

    public void SetLocalDataToSave(PlayerData data)
    {
        _mainStatBlock = data.MainStatBlock;
        
        if (data.CurrentWeapon != null)
            CurrentWeapon = data.CurrentWeapon;

        // Set other stuff based on above values.
    }

    public void UpdateTilemap(Tilemap newTilemap)
    {
        tilemap = newTilemap;
    }

    public float GetStat(StatType stat)
    {
        return (_mainStatBlock * _weaponStatBlock * _abilityStatBlock).GetValue((int)stat).Value;
    }

    public void AddStatModifier(StatBlock statChange)
    {
        _mainStatBlock *= statChange;
    }

    public void NewWeaponEquipped(StatBlock newWeaponStatBlock)
    {
        _weaponStatBlock = newWeaponStatBlock;
    }

    public void NewAbilityEquipped(PowerUp powerUp)
    {
        currentPowerUp = powerUp;
    }

    public void ClearEquippedAbility()
    {
        if (currentPowerUp = null)
            return;

        Destroy(GetComponent<PowerUp>());
        EquipmentManager.Instance.SetPowerUpCanSpawn(true);
        currentPowerUp = null;
    }

    public void ActivateAbilityStats(StatBlock stats)
    {
        _abilityStatBlock = stats;
    }

    public void DeActivateAbilityStats()
    {
        _abilityStatBlock = new(1, 1, 1, 1, 1, 1);
    }

    #region Damage and Knockback
    public void TakeDamage(float damageAmount)
    {
        KnockbackPercent += damageAmount / GetStat(StatType.DamageResistance);

        // Damage Cap
        if (KnockbackPercent > _maxDamagePercent)
            KnockbackPercent = _maxDamagePercent;

        EquipmentManager.Instance.PlayerTookDamage?.Invoke();

        AudioManager.Instance.Play("Hurt");

        SetDamageDisplay();
    }

    void SetDamageDisplay()
    {
        _desiredDamageDisplay = KnockbackPercent;
    }

    /// <summary>
    /// Applies knockback to player and prevents them from moving for a specified time.
    /// </summary>
    /// <param name="sourcePosition">The position of the entity dealing the knockback.</param>
    /// <param name="knockbackMultiplier">Intensity of knockback.</param>
    /// <param name="stunDuration">Duration in seconds that movement is prevented after being hit.</param>
    public void TakeKnockback(Vector2 sourcePosition, float knockbackMultiplier, float stunDuration)
    {
        if (_isImmune || knockbackMultiplier == 0)
        {
            Invoke(nameof(ResetKB), 0.05f);
            return;
        }

        if (EquipmentManager.Instance.CanHit())
            EquipmentManager.Instance.ToggleHit(false);
        else
        {
            EquipmentManager.Instance.ToggleHit(true);
        }

        _playerMovement.isGrounded = false;
        _isImmune = true;

        _playerCollider.enabled = false;

        Vector2 input = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical")).normalized;

        float multiplier = (1 + (KnockbackPercent / 100)) * knockbackMultiplier;
        multiplier = multiplier < 3 ? 3 : multiplier;

        Vector2 knockbackVector = ((Vector2)transform.position - sourcePosition).normalized * multiplier;

        if (knockbackVector.magnitude >= 3.5f)
            PlayKnockbackSprite();
        else
        {
            _headRenderer.sprite = _angryHead;
        }

        Vector2 diVector = input * (knockbackVector.magnitude * _diStrength);

        StartCoroutine(SetPlayerVelocityOverTime((knockbackVector + diVector) / GetStat(StatType.KnockbackResistance), stunDuration));
    }
    
    private void PlayKnockbackSprite()
    {
        for (int i = 0; i < _allSrsOnPlayer.Length; i++)
        {
            if (i == 0)
                continue;

            _allSrsOnPlayer[i].enabled = false;
        }

        _allSrsOnPlayer[0].sprite = _playerKnockbackSprite;
    }

    private void ResetKnockbackSprite()
    {
        foreach (SpriteRenderer sr in _allSrsOnPlayer)
        {
            sr.enabled = true;
        }

        _allSrsOnPlayer[0].sprite = _initialBody;
    }

    internal IEnumerator SetPlayerVelocityOverTime(Vector2 knockbackVector, float stunDuration)
    {
        foreach (Vector2 velocity in GetVelocity(knockbackVector, stunDuration))
        {
            _playerMovement.rb.velocity = velocity;
            yield return null;
        }

        ResetKnockbackSprite();
        ResetKB();
    }

    private IEnumerable<Vector2> GetVelocity(Vector2 knockbackVector, float stun)
    {
        Vector2 returningVector;

        float t = 0;

        while (t <= stun)
        {
            returningVector = knockbackVector - (knockbackVector * (t / stun));

            yield return returningVector;

            t += Time.deltaTime;
        }
    }

    void ResetKB()
    {
        _headRenderer.sprite = _initialHead;
        _playerMovement.isGrounded = true;
        _isImmune = false;
        _playerCollider.enabled = true;

        EquipmentManager.Instance.ToggleHit(true);
    }

    public void Die()
    {
        // TODO: Play splash animation and sound perhaps.

        if (waveStateMachine.CurrentState != waveStateMachine.States[WaveStateMachine.WaveState.WaveInProgress])
        {
            transform.position = Vector2.zero;
            return;
        }

        waveStateMachine.TransitionToState(WaveStateMachine.WaveState.WaveLoss);
        gameObject.SetActive(false);
    }
    #endregion
}
