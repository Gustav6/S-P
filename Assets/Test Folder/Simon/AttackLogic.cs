using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class AttackLogic : MonoBehaviour
{
    [SerializeField] private UnityEvent animationComplete;

    private AttackController _attackController;

    private CapsuleCollider2D _hitbox;

    private void Awake()
    {
        _attackController = GetComponent<AttackController>();
    }

    public void SpawnHitbox()
    {
        _hitbox = Instantiate(_attackController.CurrentWeapon.Hitbox, transform.GetChild(0).GetChild(0));
    }

    public void DespawnHitbox()
    {
        if (_hitbox != null)
            Destroy(_hitbox.gameObject);
    }

    public void AnimationFinished()
    {
        // TODO: Play SFX to show player can swing weapon again.

        animationComplete?.Invoke();
    }

    public static IEnumerator AddAttackBoost(Rigidbody2D rb, Vector2 targetDirection, float forceAmount, float forceActiveTime)
    {
        // TODO: Use knockback method.

        rb.AddForce(targetDirection * forceAmount, ForceMode2D.Impulse);

        yield return new WaitForSeconds(forceActiveTime);

        rb.velocity = Vector2.zero;
    }
}
