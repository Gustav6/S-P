using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : MonoBehaviour, IDamageable
{
    public float KnockbackPercent { get; set; }

    public void TakeKnockback()
    {
        Debug.Log($"Take knockback\nCurrent percent: {KnockbackPercent} %");
    }
}
