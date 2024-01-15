using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class PlayerBaseState
{
    internal abstract void EnterState(PlayerControllerAttackTest player);

    internal abstract void UpdateState(PlayerControllerAttackTest player);

    internal abstract void ExitState(PlayerControllerAttackTest player);
}
