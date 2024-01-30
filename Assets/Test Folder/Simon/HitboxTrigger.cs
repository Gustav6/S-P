using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class HitboxTrigger : MonoBehaviour
{
    // Event calls the attack method for player.
    [SerializeField] private UnityEvent<IDamageable> hitEvent;

    private AttackController _parentController;

    private void Awake()
    {
        _parentController = GetComponentInParent<AttackController>();
    }

    private void OnTriggerEnter2D(Collider2D triggerInfo)
    {
        var damageable = triggerInfo.GetComponent<IDamageable>();

        if (damageable == null || triggerInfo.CompareTag(_parentController.tag))
        {
            return;
        }

        hitEvent?.Invoke(damageable);
    }
}
