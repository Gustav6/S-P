using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyControllerAttackTest : MonoBehaviour, IDamageable
{
    public float KnockbackPercent { get; set; }

    public void TakeKnockback()
    {
        Debug.Log($"Current percent: {KnockbackPercent}%");
    }
}
