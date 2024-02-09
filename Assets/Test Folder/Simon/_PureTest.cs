using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class _PureTest : MonoBehaviour, IDamageable
{
    public float KnockbackPercent { get; set; }

    internal float damage = 10;
    internal float knockbackMultiplier = 1.5f;

    private Rigidbody2D _rb;
    [SerializeField] private GameObject _hitbox;

    private void Awake()
    {
        _rb = GetComponent<Rigidbody2D>();
    }

    public void TakeKnockback(Vector2 sourcePosition, float knockbackMultiplier, float stunDuration)
    {
        _rb.velocity = (Vector2.left * ((5 + (KnockbackPercent / 100)) * knockbackMultiplier));
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.L) && !isAttacking)
            StartCoroutine(Attacking());
    }

    [SerializeField] bool isAttacking;

    private IEnumerator Attacking()
    {
        Debug.Log("Enemy is attacking");
        isAttacking = true;
        _hitbox.SetActive(true);

        yield return new WaitForSeconds(2);

        _hitbox.SetActive(false);
        isAttacking = false;
    }
}
