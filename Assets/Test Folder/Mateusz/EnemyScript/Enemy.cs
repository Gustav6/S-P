
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    public float _diStrength = 0.25f;
    public float _kbResistance;

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

}
    
  


