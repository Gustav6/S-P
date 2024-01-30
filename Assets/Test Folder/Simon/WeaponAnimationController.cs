using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class WeaponAnimationController : MonoBehaviour
{
    [SerializeField] private UnityEvent animationComplete;
    [SerializeField] private GameObject thisGameObject;

    private AttackController _attackController;

    private CapsuleCollider2D _hitbox;

    // Only used if the weapon resests after initial swing.
    private bool _isFirstHit;

    private void Awake()
    {
        _attackController = thisGameObject.GetComponent<AttackController>();
    }

    public void SpawnHitbox()
    {
        // Makes sure a hitbox doesn't spawn if a weapon is playing animation as reset rather than hit animation.
        if (_isFirstHit && _attackController.CurrentWeapon.IsWeaponResetable)
        {
            _isFirstHit = false;
            return;
        }
        else
        {
            _isFirstHit = true;
        }

        _hitbox = Instantiate(_attackController.CurrentWeapon.Hitbox, thisGameObject.transform.GetChild(0));
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
