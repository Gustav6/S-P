using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
using TMPro;
using System.Runtime.CompilerServices;
using UnityEngine.UI;

// Simon & Jacob
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
    [SerializeField] TMP_Text _scoreDisplayText;
    [SerializeField] TMP_Text _damageDisplayText;
    public Gradient DamageGradient;
    float _currentDamageDisplay;
    float _desiredDamageDisplay;

    bool _isImmune = false;
    bool _isInvulnerable = false;

    [SerializeField] float _maxDamagePercent = 300f;
    [SerializeField] ParticleSystem _healParticlesPrefab;
    public float KnockbackPercent { get; set; }

    // Not used at the moment.
    public int ConsecutiveHits { get; set; }

    public PowerUp currentPowerUp { get; private set; }
    [SerializeField] private Image _powerUpHudElement;
    public bool PowerupActive { get; private set; }

    public int Score { get; private set; }
    public int WaveNumber { get; private set; }

    [SerializeField] Animator _powerupAnim;

    public void AddScore(int amount)
    {
        Score += amount;
        _scoreDisplayText.text = Score.ToString();
    }

    public void SetWaveNumber(int number)
    {
        WaveNumber = number;
    }

    private void Start()
    {
        SetWaveNumber(1);
        _data = SaveSystem.Instance.LoadGameSave();
        SetLocalDataToSave(_data);

        _tiles = IDamageable.PopulateTilesDictonary(tilemap);

        _powerUpHudElement.color = new Color(1, 1, 1, 0);

        // Call this when you want to change the player weapon n stuff.
        EquipmentManager.Instance.SwitchWeapon(CurrentWeapon);

        if (EquipmentManager.Instance.OnPowerUpEquipped == null)
            EquipmentManager.Instance.OnPowerUpEquipped = PowerupEquipped;

        if (EquipmentManager.Instance.OnPowerUpUsed == null)
            EquipmentManager.Instance.OnPowerUpUsed = PowerupUsed;
    }

    private void Update()
    {
        _thisDamagable.CheckDeath(tilemap, _tiles, _playerCollider);

        _currentDamageDisplay = Mathf.Lerp(_currentDamageDisplay, _desiredDamageDisplay, Time.deltaTime * 10);
        _damageDisplayText.text = ((int)_currentDamageDisplay).ToString() + "%";
        _damageDisplayText.color = DamageGradient.Evaluate(_currentDamageDisplay / _maxDamagePercent);

        if (_currentDamageDisplay >= 299 && KnockbackPercent == 300)
            _damageDisplayText.text = "300%";
    }

    // Simon
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
        _tiles = IDamageable.PopulateTilesDictonary(tilemap);
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
        _powerupAnim.Play("PowerupEquipped");
    }

    public void ClearEquippedAbility()
    {
        if (currentPowerUp = null)
            return;

        _powerupAnim.speed = 1;
        _powerupAnim.Play("PowerupCleared");
        DeActivateAbilityStats();
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

    // Simon
    private void PowerupEquipped()
    {
        _powerUpHudElement.color = new Color(1, 1, 1, 1);
        _powerUpHudElement.sprite = currentPowerUp.powerUpSprite;
        PowerupActive = false;
    }

    private void PowerupUsed()
    {
        if (currentPowerUp.duration > 0)
        {
            _powerupAnim.speed = 1 / currentPowerUp.duration;
            _powerupAnim.Play("PowerupDuration");
        }

        PowerupActive = true;
    }

    #region Damage and Knockback
    // Simon
    public void TakeDamage(float damageAmount)
    {
        KnockbackPercent += damageAmount / GetStat(StatType.DamageResistance);

        // Damage Cap
        if (KnockbackPercent > _maxDamagePercent)
            KnockbackPercent = _maxDamagePercent;

        EquipmentManager.Instance.PlayerTookDamage?.Invoke();

        AudioManager.Instance.PlaySound("Hurt");

        SetDamageDisplay();
    }

    public void HealDamage(float healAmount)
	{
        KnockbackPercent -= healAmount;

        if (KnockbackPercent < 0)
            KnockbackPercent = 0;

        ParticleManager.Instance.SpawnParticle(_healParticlesPrefab, transform.position, transform);
        SetDamageDisplay();
	}

    void SetDamageDisplay()
    {
        _desiredDamageDisplay = KnockbackPercent;
    }

    public void ResetDamage()
    {
        KnockbackPercent = 0;
        SetDamageDisplay();
    }

    // Simon
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

        EquipmentManager.Instance.ToggleHit(false);

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

    // Simon
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

    // Simon
    private void ResetKnockbackSprite()
    {
        foreach (SpriteRenderer sr in _allSrsOnPlayer)
        {
            sr.enabled = true;
        }

        _allSrsOnPlayer[0].sprite = _initialBody;
    }

    // Simon
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

    // Simon
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
        EquipmentManager.Instance.ToggleHit(true);

        _headRenderer.sprite = _initialHead;
        _playerMovement.isGrounded = true;
        _isImmune = false;
        _playerCollider.enabled = true;
    }

    // Simon
    public void Die()
    {
        if (_isInvulnerable)
            return;

        ParticleManager.Instance.SpawnWaterSplash(transform.position);
        AudioManager.Instance.PlaySound("WaterSplash");

        if (waveStateMachine.CurrentState != waveStateMachine.States[WaveStateMachine.WaveState.WaveInProgress])
        {
            transform.position = tilemap.transform.GetChild(2).position;
            return;
        }

        waveStateMachine.TransitionToState(WaveStateMachine.WaveState.WaveLoss);
        gameObject.SetActive(false);
    }

    public void ToggleInvulnerability(bool value)
	{
        _isInvulnerable = value;
    }
    #endregion
}
