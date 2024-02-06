using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class smallEnemy : Enemy, IDamageable
{
    public float KnockbackPercent { get; set; }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.K))
        {
            TakeKnockback(Vector2.zero, 10, 0.25f);
        }
    }

    public void TakeKnockback(Vector2 sourcePosition, float knockbackMultiplier, float stunDuration)
    {
        if (_isImmune)
            return;

        _isGrounded = false;
        _isImmune = true;

        Vector2 input = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical")).normalized;

        Vector2 knockbackVector = ((Vector2)transform.position - sourcePosition).normalized * knockbackMultiplier;

        Vector2 diVector = input * (knockbackVector.magnitude * _diStrength);

        rb.AddForce(knockbackVector + diVector, ForceMode2D.Impulse);

        Invoke(nameof(ResetKB), stunDuration);

        Debug.Log("hit");
    }

    void ResetKB()
    {
        _isGrounded = true;
        _isImmune = false;
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "WaterCollision")
        {
            Debug.Log("Man overboard");
            Destroy(this.gameObject);
        }
    }
}
