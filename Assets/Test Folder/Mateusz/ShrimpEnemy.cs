using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShrimpEnemy : Enemy, IDamageable
{
    public float KnockbackPercent { get; set; }

    Enemy enemy;

    private void Start()
    {
         enemy = FindFirstObjectByType< Enemy>();

        rb = GetComponent<Rigidbody2D>();
    }

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

        Vector2 knockbackVector = ((Vector2)transform.position - sourcePosition).normalized * knockbackMultiplier;

        rb.AddForce(knockbackVector, ForceMode2D.Impulse);

        Invoke(nameof(ResetKB), stunDuration);
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

        if (collision.gameObject.tag == "Player")
        {
            TakeKnockback(Vector2.zero, 10, 0.25f);
            Debug.Log("Collided with player");
        }
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
            TakeKnockback(Vector2.zero, 5, 0.25f);
            Debug.Log("Collided with player");
    }
}
