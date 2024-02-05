using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class smallEnemy : Enemy, IDamageable
{
    public float KnockbackPercent { get; set; }

    private void Awake()
    {
        anim = transform.GetChild(0).GetComponent<Animator>();
    }
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.K))
        {
            TakeKnockback(Vector2.zero, 10, 0.25f);
        }

        anim.SetBool("isMoving", true);

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

    private void OnCollisionEnter2D(Collision2D other)
    {
        Debug.Log("Collision");
        if (other.gameObject.tag == "Weapon")
        {
            TakeKnockback(Vector2.zero, 20, 0.25f);
            Debug.Log("Collided w player");
        }

        if (other.gameObject.tag == "Player")
        {
            player.TakeKnockback(Vector2.zero, 2, 0.25f);
            Debug.Log("player took knockback");
        }
    }


    void ResetKB()
    {
        _isGrounded = true;
        _isImmune = false;
    }
}
