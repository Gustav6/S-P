using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerData
{
    public StatBlock MainStatBlock;
    public WeaponSO CurrentWeapon;

    public PlayerData()
    {
        MainStatBlock = new StatBlock(1, 1, 1, 1, 1, 1);
        CurrentWeapon = null;
    }
}
