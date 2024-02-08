using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShrimpEnemy : Enemy, IDamageable
{
    public float KnockbackPercent { get; set; }

    Enemy _enemy;

    private void Start()
    {
         _enemy = FindFirstObjectByType< Enemy>();
         _player = FindFirstObjectByType<PlayerMovement>();
         _rb = GetComponent<Rigidbody2D>();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.K))
        {
            TakeKnockback(Vector2.zero, 2, 0.25f);
        }

        if (Input.GetKeyDown(KeyCode.F))
        {
            OnHit(_kbPower);
        }
    }
    public void TakeKnockback(Vector2 sourcePosition, float knockbackMultiplier, float stuntDuration)
    {
        Vector2 knockbackVector = (transform.localPosition - _player.transform.localPosition).normalized;

        _rb.AddForce(knockbackVector * _kbPower, ForceMode2D.Impulse);

        Debug.Log("knockback applied " +knockbackVector);
    }

    private void OnHit(float kbPower)
    {
        _kbPower = (kbPower * 1.06f);

    }

    //void OnTriggerEnter2D(Collider2D collision)
    //{
    //    if (collision.gameObject.tag == "WaterCollision")
    //    {
    //        Debug.Log("Man overboard");
    //        Destroy(this.gameObject);
    //    }

    //    if (collision.gameObject.tag == "Player")
    //    {
    //        TakeKnockback(Vector2.zero, 10, 0.25f);
    //        Debug.Log("Collided with player");
    //    }
    //}
}
