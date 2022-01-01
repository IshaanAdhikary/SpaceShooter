using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class PlayerMovement : MonoBehaviour
{
    public RoundManager gameScript;
    public GameObject pauseTint;
    public GameObject bulletTemplate;
    public GameObject canvas;
    public GameObject HealthBar;
    public Color theme;

    public float ROFMulti;
    public float damageMulti;
    public float speedMulti;
    public float healthMulti;

    public float Health;

    public bool isWinner;
    public bool isDead;

    private Image healthImage;
    private Camera cam;
    private CharacterController2D controller;
    private CameraController cameraScript;
    private Vector2 moveDirection = Vector2.zero;
    private Vector2 shootDirection = Vector2.zero;

    private float startingHealth = 100f;
    private float shotTimer = 0f;
    private float baseMoveSpeed = 8f;
    private float bulletROF = 400f;
    private float bulletDamage = 4f;

    void Awake()
    {
        cam = Camera.main;
        controller = GetComponent<CharacterController2D>();
        cameraScript = cam.GetComponent<CameraController>();
        canvas.GetComponent<Canvas>().worldCamera = cam;
    }

    private void Start()
    {
        healthImage = HealthBar.GetComponent<Image>();
        Health = startingHealth * healthMulti;
    }

    private void Update()
    {
        canvas.transform.position = transform.position;

        healthImage.fillAmount = Health / startingHealth;
        if (Health <= 0)
        {
            Die();
        }

        if (cameraScript.players.Count == 1 && cameraScript.players.Contains(gameObject))
        {
            gameScript.Win(gameObject);
        }
    }

    private void FixedUpdate()
    {
        controller.Move(moveDirection * baseMoveSpeed * speedMulti);

        if (shootDirection != Vector2.zero)
        {
            if (shotTimer >= 60 / (bulletROF * ROFMulti))
            {
                ShootSmall();
                shotTimer = 0f;
            }
            shotTimer += Time.fixedDeltaTime;
        }
    }

    private void ShootSmall()
    {
        GameObject bullet = Instantiate(bulletTemplate, transform.position + new Vector3(shootDirection.x, shootDirection.y, 0), Quaternion.identity);
        BulletScript script = bullet.GetComponent<BulletScript>();
        script.firedBy = gameObject.name;
        script.damage = bulletDamage;
        script.velocity = (shootDirection) * (bulletROF * ROFMulti)/25f;
    }

    private void Die()
    {
        cameraScript.removeFromCam(gameObject);
        canvas.SetActive(false);
        gameObject.SetActive(false);
    }

    public void Shoot(InputAction.CallbackContext context)
    {
        shootDirection = context.ReadValue<Vector2>().normalized;
    }

    public void Move(InputAction.CallbackContext context)
    {
        moveDirection = context.ReadValue<Vector2>();
    }

    public void Pause()
    {
        if (pauseTint) { pauseTint.SetActive(true); }
        Time.timeScale = 0f;
    }
    
    public void Continue()
    {
        if (gameScript.gameEnded)
        {
            Scene scene = SceneManager.GetActiveScene();
            SceneManager.LoadScene(scene.name);
        }
    }
}
