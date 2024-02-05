using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCollisionTest : MonoBehaviour
{
    Rigidbody2D rb;
    public GameObject player;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "water")
        {
            player.SetActive(false);
        }
    }
}
