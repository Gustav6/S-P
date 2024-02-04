using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class AttackLogic : MonoBehaviour
{
    [SerializeField] private UnityEvent animationComplete;
    [SerializeField] private Transform weaponSpawnParent;

    private AttackController _attackController;

    private CapsuleCollider2D _hitbox;

    private void Awake()
    {
        _attackController = GetComponent<AttackController>();
    }

    public void SpawnHitbox()
    {
        _hitbox = Instantiate(_attackController.CurrentWeapon.Hitbox, weaponSpawnParent);
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

    public static IEnumerator AddAttackBoost(PlayerMovement player, Rigidbody2D rb, Vector2 targetDirection, float forceAmount, float forceActiveTime)
    {
        player.ToggleMovementLock();

        rb.AddForce(targetDirection * forceAmount, ForceMode2D.Impulse);

        yield return new WaitForSeconds(forceActiveTime);

        rb.velocity = Vector2.zero;

        player.ToggleMovementLock();
    }
}
