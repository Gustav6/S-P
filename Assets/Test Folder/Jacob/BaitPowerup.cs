using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaitPowerup : PowerUp
{
    GameObject _baitObject;

    public override void OnDeactivatePowerUp()
    {

    }

    public override void UsePowerUp()
    {
        Instantiate(_baitObject, transform.position, Quaternion.identity);
    }
}
