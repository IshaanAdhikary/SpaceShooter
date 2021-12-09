using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class SceneManager : MonoBehaviour
{
    public GameObject star;

    private PlayerInputManager inputManager;
    private Camera mainCamera;
    private CameraController cameraScript;
    private int playerCount = 0;

    private void Awake()
    {
        mainCamera = Camera.main;
        inputManager = GetComponent<PlayerInputManager>();
        cameraScript = mainCamera.GetComponent<CameraController>();
    }

    private void Start()
    {
        int clusterCount = Random.Range(120, 150);

        for (int i = 0; i < clusterCount; i++)
        {
            float xPos = Random.Range(-19f, 19f);
            float yPos = Random.Range(-14f, 14f);
            float scale = Random.Range(0.6f, 1f);

            GameObject genStar = Instantiate(star, new Vector3(xPos, yPos), Quaternion.identity);
            genStar.transform.localScale = new Vector2(scale, scale);
            genStar.transform.parent = gameObject.transform;
        }
    }

    public void OnPlayerJoined(PlayerInput playerInput)
    {
        playerCount++;
        SpriteRenderer playerSprite = playerInput.GetComponent<SpriteRenderer>();

        switch (playerCount)
        {
            case 1:
                playerInput.name = "Player1";
                cameraScript.player1 = playerInput.gameObject;
                break;
            case 2:
                playerInput.name = "Player2";
                playerSprite.color = new Color(1, 0.47f, 0.53f);
                cameraScript.player2 = playerInput.gameObject;
                inputManager.DisableJoining();
                break;
        }
    }
}
