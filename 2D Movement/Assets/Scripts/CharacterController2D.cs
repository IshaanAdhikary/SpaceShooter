using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterController2D : MonoBehaviour
{
    private Rigidbody2D rb;
    private Vector2 velocity;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    public void Move(Vector2 move)
    {
        rb.velocity = Vector2.SmoothDamp(rb.velocity, move, ref velocity, 0.4f);
    }
}
