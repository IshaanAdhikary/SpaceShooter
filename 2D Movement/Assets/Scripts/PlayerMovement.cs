using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    public GameObject bulletTemplate;
    public GameObject canvas;
    public GameObject HealthBar;
    public float ROFMulti;
    public float speedMulti;
    public float healthMulti;
    public float Health;

    private Image healthImage;
    private Camera cam;
    private CharacterController2D controller;
    private Vector2 moveDirection = Vector2.zero;
    private Vector2 shootDirection = Vector2.zero;
    private float startingHealth = 100f;
    private float shotTimer = 0f;
    private float baseMoveSpeed = 8f;
    private float bulletROF = 400f;
    private float bulletDamage = 4f;

    void Awake()
    {
        controller = GetComponent<CharacterController2D>();
        cam = Camera.main;
        canvas.GetComponent<Canvas>().worldCamera = cam;
        healthImage = HealthBar.GetComponent<Image>();
        Health = startingHealth * healthMulti;
    }

    private void Update()
    {
        healthImage.fillAmount = Health / startingHealth;
        if (Health <= 0)
        {
            Destroy(gameObject);
        }
    }

    private void FixedUpdate()
    {
        controller.Move(moveDirection * baseMoveSpeed * speedMulti);

        if (shootDirection != Vector2.zero)
        {
            if (shotTimer >= 60 / (bulletROF * ROFMulti))
            {
                ShootGun();
                shotTimer = 0f;
            }
            shotTimer += Time.fixedDeltaTime;
        }

        float xClamp = Mathf.Clamp(transform.position.x,
            cam.ScreenToWorldPoint(Vector3.zero).x + (transform.localScale.x * 1.33f / 2),
            cam.ScreenToWorldPoint(new Vector3 (cam.pixelWidth, 0, 0)).x - (transform.localScale.x * 1.3f / 2));
        float yClamp = Mathf.Clamp(transform.position.y,
            cam.ScreenToWorldPoint(Vector3.zero).y + (transform.localScale.y * 1.33f/ 2),
            cam.ScreenToWorldPoint(new Vector3(0, cam.pixelHeight, 0)).y - (transform.localScale.y * 1.3f / 2));
        transform.position = new Vector2 (xClamp, yClamp);
    }

    private void ShootGun()
    {
        GameObject bullet = Instantiate(bulletTemplate, transform.position + new Vector3(shootDirection.x, shootDirection.y, 0), Quaternion.identity);
        BulletScript script = bullet.GetComponent<BulletScript>();
        script.firedBy = gameObject.name;
        script.damage = bulletDamage;
        script.velocity = (shootDirection) * (bulletROF * ROFMulti)/25f;
    }

    public void Shoot(InputAction.CallbackContext context)
    {
        shootDirection = context.ReadValue<Vector2>().normalized;
    }

    public void Move(InputAction.CallbackContext context)
    {
        moveDirection = context.ReadValue<Vector2>();
    }
}
