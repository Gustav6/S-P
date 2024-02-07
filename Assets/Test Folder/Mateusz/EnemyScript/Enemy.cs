
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    public float _kbPower;
 
    [HideInInspector]
    public bool _isImmune = false;
    public bool _isGrounded = true;
    public bool _movementLocked = false;

    [HideInInspector]
    public PlayerMovement _player;

    [HideInInspector]
    public Rigidbody2D _rb;

    [HideInInspector]
    public Animator _anim;
   
    private void Start()
    {
        _rb = GetComponent<Rigidbody2D>();

        _player = FindFirstObjectByType<PlayerMovement>();

    }

    //ublic void OnHit(float kbResistance, Vector2 knockback)
    //{
    //    if (!immune)
    //    {
    //        totalKbResistance -= kbResistance;

    //        rb.AddForce(knockback, ForceMode2D.Impulse);

    //        if (immunityTimer)
    //        {
    //            immune = true;
    //        }
    //    }
    //}

}
    
  


