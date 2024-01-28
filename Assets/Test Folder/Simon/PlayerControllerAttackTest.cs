using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerControllerAttackTest : MonoBehaviour
{
    #region States
    private PlayerBaseState _currentState;

    internal PlayerDefaultState defaultState = new();
    internal PlayerAttackingState attackState = new();
    #endregion

    // TODO: Remove states and rework with bools or something

    public TestWeaponSO CurrentWeapon;

    internal Animator weaponAnimator;

    private void Awake()
    {
        weaponAnimator = GetComponentInChildren<Animator>();
    }

    private void Start()
    {
        _currentState = defaultState;

        _currentState.EnterState(this);
    }

    private void Update()
    {
        _currentState.UpdateState(this);
    }

    /// <summary>
    /// Switch the player state.
    /// </summary>
    public void SwitchState(PlayerBaseState stateToChange)
    {
        _currentState.ExitState(this);

        _currentState = stateToChange;

        stateToChange.EnterState(this);
    }

    /// <summary>
    /// Switch the player state from the attacking state to the default state.
    /// Used to escape the state when an attack animation is finished playing.
    /// </summary>
    public void LeaveAttackState()
    {
        _currentState.ExitState(this);

        _currentState = defaultState;

        defaultState.EnterState(this);
    }

    public void Attack(IDamageable damageable)
    {
        damageable.TakeDamage(CurrentWeapon.Damage);
        damageable.TakeKnockback();
    }
}
