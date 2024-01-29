using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class WeaponAnimationController : MonoBehaviour
{
    [SerializeField] private UnityEvent animationComplete;
    [SerializeField] private GameObject player;

    private PlayerControllerAttackTest _playerController;

    private CapsuleCollider2D _hitbox;

    // Only used if the weapon resests after initial swing.
    private bool _isFirstHit;

    private void Awake()
    {
        _playerController = player.GetComponent<PlayerControllerAttackTest>();
    }

    public void SpawnHitbox()
    {
        // Makes sure a hitbox doesn't spawn if a weapon is playing animation as reset rather than hit animation.
        if (_playerController.CurrentWeapon.IsWeaponResetable && _isFirstHit)
        {
            _isFirstHit = false;
            return;
        }
        else
        {
            _isFirstHit = true;
        }

        _hitbox = Instantiate(_playerController.CurrentWeapon.Hitbox, player.transform.GetChild(0));
    }

    public void DespawnHitbox()
    {
        if (_hitbox != null)
            Destroy(_hitbox.gameObject);
    }

    // TODO: Add methods for sound effects and visual effects like camera shake for events.

    public void AnimationFinished()
    {
        animationComplete?.Invoke();
    }
}
