using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class AnimationTest : MonoBehaviour
{
    // TODO: Rotate the sprite anchor to the mouse.

    [SerializeField] private UnityEvent animationComplete;
    [SerializeField] private GameObject player;

    private PlayerControllerAttackTest _playerController;

    private CircleCollider2D _hitbox;

    private void Awake()
    {
        _playerController = player.GetComponent<PlayerControllerAttackTest>();
    }

    public void SpawnHitbox()
    {
        _hitbox = Instantiate(_playerController.CurrentWeapon.Hitbox, player.transform.position + (Vector3)_playerController.CurrentWeapon.HitboxToPlayerOffset, Quaternion.identity, player.transform);
    }

    public void DespawnHitbox()
    {
        Destroy(_hitbox.gameObject);
    }

    // TODO: Add methods for sound effects and visual effects like camera shake for events.

    public void AnimationFinished()
    {
        animationComplete?.Invoke();
    }
}
