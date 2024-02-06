using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStats : MonoBehaviour
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
    }

    #endregion

    StatBlock _mainStatBlock = new(1, 1, 1, 1, 1, 1, 1);

    StatBlock _weaponStatBlock;

    public float GetStat(StatType stat)
    {
        return (_mainStatBlock * _weaponStatBlock).GetValue(stat).Value;
    }

    public void AddStatModifier(StatBlock statChange)
    {
        _mainStatBlock *= statChange;
    }

    public void NewWeaponEquipped(StatBlock newWeaponStatBlock)
    {
        _weaponStatBlock = newWeaponStatBlock;
    }
}
