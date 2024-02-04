using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestEnemyAttack : MonoBehaviour, IDamageable
{
    public float KnockbackPercent { get; set; }

    public void TakeKnockback(Vector2 sourcePosition, float knockbackMultiplier, float stunDuration)
    {
        Debug.Log($"Current enemy percent: {KnockbackPercent}% Enemy knockback as: {KnockbackPercent * knockbackMultiplier}\n" +
                  $"Hit from: {sourcePosition} and stunned for: {stunDuration}");
    }
}
