using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerDefaultState : PlayerBaseState
{
    internal override void EnterState(PlayerControllerAttackTest player)
    {

    }

    internal override void UpdateState(PlayerControllerAttackTest player)
    {
        if (Input.GetKey(KeyCode.Space))
        {
            player.SwitchState(player.attackState);
        }
    }

    internal override void ExitState(PlayerControllerAttackTest player)
    {

    }
}
