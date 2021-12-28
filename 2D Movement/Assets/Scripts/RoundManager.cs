using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using TMPro;

public class RoundManager : MonoBehaviour
{
    public GameObject star;
    public GameObject winTextObj;
    public Vector2 playSize;
    public bool gameEnded = false;
    public bool P2 = false;

    private PlayerInputManager inputManager;
    private Camera mainCamera;
    private CameraController cameraScript;
    private TextMeshProUGUI winText;
    private int playerCount = 0;

    // Delete this later
    private Color[] playerColors = new Color[4] {new Color(0.5f, 1, 0.87f), new Color(1, 0.5f, 0.5f), new Color(1, 1, 0.5f), new Color(0.5f, 1, 0.5f) };

    private void Awake()
    {
        mainCamera = Camera.main;
        inputManager = GetComponent<PlayerInputManager>();
        cameraScript = mainCamera.GetComponent<CameraController>();
        winText = winTextObj.GetComponent<TextMeshProUGUI>();
    }

    private void Start()
    {
        int areaSize = Mathf.RoundToInt(playSize.x * playSize.y);
        int clusterCount = Random.Range(Mathf.RoundToInt(areaSize / 10), Mathf.RoundToInt(areaSize / 8));

        for (int i = 0; i < clusterCount; i++)
        {
            float xPos = Random.Range(-playSize.x/2 + 1, playSize.x/2 - 1);
            float yPos = Random.Range(-playSize.y/2 + 1, playSize.y/2 - 1);
            float scale = Random.Range(0.6f, 1f);

            GameObject genStar = Instantiate(star, new Vector3(xPos, yPos), Quaternion.identity);
            genStar.transform.localScale = new Vector2(scale, scale);
            genStar.transform.parent = gameObject.transform;
        }
    }

    public void OnPlayerJoined(PlayerInput playerInput)
    {
        playerCount++;

        GameObject player = playerInput.gameObject.transform.GetChild(0).gameObject;
        SpriteRenderer playerSprite = player.GetComponent<SpriteRenderer>();
        PlayerMovement playerMovement = player.GetComponent<PlayerMovement>();

        playerMovement.gameScript = this;

        playerInput.currentActionMap = playerInput.actions.FindActionMap("Player");

        // Delete this later
        switch (playerCount)
        {
            case 2:
                P2 = true;
                break;
            case 4:
                inputManager.DisableJoining();
                break;
        }

        string objName = "Player " + playerCount.ToString();
        playerInput.name = objName;
        player.name = objName + " Body";
        playerSprite.color = playerColors[playerCount - 1];
        playerMovement.playerNum = playerCount;
        cameraScript.addToCam(player);
    }

    public void Win(GameObject winner, int winNumber)
    {
        PlayerMovement winnersScript = winner.GetComponent<PlayerMovement>();
        winnersScript.isWinner = true;

        winTextObj.SetActive(true);
        winText.color = playerColors[winNumber - 1];
        winText.text = winner.transform.parent.gameObject.name + " wins!";
        gameEnded = true;

        // Delete this later
        inputManager.DisableJoining();
    }
}
