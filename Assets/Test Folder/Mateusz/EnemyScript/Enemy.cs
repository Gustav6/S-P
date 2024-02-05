
using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography.X509Certificates;
using TMPro;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    public float _diStrength = 0.25f;

    public bool _isImmune = false;
    [HideInInspector]
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

        player = FindObjectOfType<PlayerMovement>();
    }

}
    
  


