using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletScript : MonoBehaviour
{
    public string firedBy;
    public float damage;
    public Vector2 velocity;

    private Rigidbody2D rb;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    private void FixedUpdate()
    {
        rb.velocity = velocity;
    }

    private void OnTriggerEnter2D(Collider2D col)
    {
        if (col.CompareTag("Wall"))
        {
            Destroy(gameObject);
        }
        else if (col.CompareTag("Player") && firedBy != col.gameObject.name)
        {
            PlayerMovement script = col.GetComponent<PlayerMovement>();
            script.Health -= damage;
            Rigidbody2D playerRB = col.GetComponent<Rigidbody2D>();
            Vector3 relativeBullet = transform.position - col.transform.position;
            playerRB.AddForceAtPosition(rb.velocity * 0.8f, transform.position - relativeBullet * 0.5f);

            Destroy(gameObject);
        }
    }
}
