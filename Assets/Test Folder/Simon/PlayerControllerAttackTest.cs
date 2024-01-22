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

    public TestWeaponSO CurrentWeapon;

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

    public void Attack(IDamageable damageable)
    {
        damageable.TakeDamage(CurrentWeapon.Damage);
        damageable.TakeKnockback();
    }
}
