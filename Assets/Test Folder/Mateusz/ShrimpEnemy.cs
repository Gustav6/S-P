using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShrimpEnemy : Enemy, IDamageable
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
        

        Vector2 input = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical")).normalized;

        Vector2 knockbackVector = ((Vector2)transform.position - sourcePosition).normalized * knockbackMultiplier;

        Vector2 diVector = input * (knockbackVector.magnitude * _diStrength);

        rb.AddForce(knockbackVector + diVector, ForceMode2D.Impulse);

        Debug.Log("hit");
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
