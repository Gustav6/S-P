using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class AttackLogic : MonoBehaviour
{
    [SerializeField] private UnityEvent animationComplete;
    [SerializeField] private Transform weaponSpawnParent;

    private AttackController _attackController;

    private GameObject _hitbox;

    private void Awake()
    {
        _attackController = GetComponent<AttackController>();
    }

    public void SpawnHitbox()
    {
        ScreenShake.instance.Shake(0.3f, 0.2f, _attackController.transform);

        _hitbox = Instantiate(_attackController.CurrentWeapon.Hitbox, weaponSpawnParent);

        if (_attackController.CurrentWeapon.HitboxVelocity != 0)
            StartCoroutine(MoveHitbox(_attackController.CurrentWeapon.HitboxVelocity));
    }

    private IEnumerator MoveHitbox(float velocity)
    {
        if (_hitbox == null)
            yield break;

        _hitbox.transform.parent = null;
        _hitbox.transform.rotation = Quaternion.AngleAxis(AimController.PointToRotation(_hitbox.transform.position), Vector3.forward);

        while (_hitbox != null)
        {
            _hitbox.transform.position += _hitbox.transform.TransformDirection(Vector3.down) * velocity;
            yield return null;
        }
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
        if (player.MovementLocked)
            yield break;

        player.ToggleMovementLock();

        rb.AddForce(targetDirection * forceAmount, ForceMode2D.Impulse);

        yield return new WaitForSeconds(forceActiveTime);

        rb.velocity = Vector2.zero;

        player.ToggleMovementLock();
    }
}
