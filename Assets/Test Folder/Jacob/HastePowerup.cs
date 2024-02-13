using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HastePowerup : PowerUp
{
    public override void UsePowerUp()
    {
        PlayerStats.Instance.ActivateAbilityStats(new StatBlock(1.25f, 1, 1, 1, 1, 1.25f));
        Invoke(nameof(RemoveStats), 10f);
    }

    void RemoveStats()
    {
        PlayerStats.Instance.DeActivateAbilityStats();
    }
}
