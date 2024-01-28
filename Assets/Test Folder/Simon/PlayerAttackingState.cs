using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAttackingState : PlayerBaseState
{
    internal override void EnterState(PlayerControllerAttackTest player)
    {
        player.weaponAnimator.SetTrigger("PlayHit");
        player.weaponAnimator.SetFloat("s", player.CurrentWeapon.AnimationSpeed);
    }

    internal override void UpdateState(PlayerControllerAttackTest player)
    {

    }

    internal override void ExitState(PlayerControllerAttackTest player)
    {

    }
}
