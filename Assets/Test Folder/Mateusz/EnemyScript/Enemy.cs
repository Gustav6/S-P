
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    public float _diStrength = 0.25f;
    public float _kbResistance;
    public float _knockbackForce = 10f;

    [HideInInspector]
    public bool _isImmune = false;
    public bool _isGrounded = true;
    public bool _movementLocked = false;

    [HideInInspector]
    public PlayerMovement player;

    [HideInInspector]
    public Rigidbody2D rb;

    [HideInInspector]
    public Animator anim;




   
    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();

        player = FindFirstObjectByType<PlayerMovement>();

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
    
  


