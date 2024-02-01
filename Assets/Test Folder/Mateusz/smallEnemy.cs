using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class smallEnemy : Enemy, IDamageable
{
    public float KnockbackPercent { get; set; }


    public void TakeKnockback()
    {
        Debug.Log($"Take knockback\nCurrent percent: {KnockbackPercent} %");
    }

    public void KnockbackDamage()
    {

    }

}
