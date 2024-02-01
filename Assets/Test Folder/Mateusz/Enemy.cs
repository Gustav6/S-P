using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public  class Enemy : MonoBehaviour
{
    [SerializeField] float _diStrength = 0.25f;

    bool _isImmune = false;
    bool _isGrounded = true;
    bool _movementLocked = false;

    Rigidbody2D rb;

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }


    protected virtual void ApplyKnockback(Vector2 sourcePosition, float strength, float stunDuration)
    {
        if (_isImmune)
            return;

        _isGrounded = false;
        _isImmune = true;
      
        Vector2 knockbackVector = ((Vector2)transform.position - sourcePosition).normalized * strength;

        Invoke(nameof(ResetKB), stunDuration);
    }

    void ResetKB()
    {
        _isGrounded = true;
        _isImmune = false;
    }

    public void ToggleMovementLock()
    {
        _movementLocked = !_movementLocked;
    }





}
