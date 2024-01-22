using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class HitboxTrigger : MonoBehaviour
{
    [SerializeField] private UnityEvent<IDamageable> hitEvent;

    private void OnTriggerEnter2D(Collider2D triggerInfo)
    {
        var damageable = triggerInfo.GetComponent<IDamageable>();

        if (damageable == null)
        {
            return;
        }

        hitEvent?.Invoke(damageable);
    }
}
