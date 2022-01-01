using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterController2D : MonoBehaviour
{
    private Rigidbody2D rb;
    private Vector2 velocity;
    private float rotVelocity;
    private Camera cam;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        cam = Camera.main;
    }

    public void Move(Vector2 move)
    {
        // Move Character
        rb.velocity = Vector2.SmoothDamp(rb.velocity, move, ref velocity, 0.4f);

        // Keep within Camera View
        float xClamp = Mathf.Clamp(transform.position.x,
            cam.ScreenToWorldPoint(Vector3.zero).x + (transform.localScale.x * 1.33f / 2),
            cam.ScreenToWorldPoint(new Vector3(cam.pixelWidth, 0, 0)).x - (transform.localScale.x * 1.3f / 2));
        float yClamp = Mathf.Clamp(transform.position.y,
            cam.ScreenToWorldPoint(Vector3.zero).y + (transform.localScale.y * 1.33f / 2),
            cam.ScreenToWorldPoint(new Vector3(0, cam.pixelHeight, 0)).y - (transform.localScale.y * 1.3f / 2));
        transform.position = new Vector2(xClamp, yClamp);

        // Slow Down Torque
        rb.angularVelocity = Mathf.SmoothDamp(rb.angularVelocity, 0, ref rotVelocity, 2);
    }
}
