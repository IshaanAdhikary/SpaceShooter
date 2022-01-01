using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.InputSystem;
using TMPro;

public class RoundManager : MonoBehaviour
{
    public GameObject playerPrefab;
    public GameObject star;
    public GameObject winTextObj;
    public GameObject pauseTint;
    public Vector2 playSize;
    public bool gameEnded = false;

    private List<Transform> playerSpawns;
    private PlayerInputManager inputManager;
    private Camera mainCamera;
    private CameraController cameraScript;
    private TextMeshProUGUI winText;

    // Delete this later

    private void Awake()
    {
        mainCamera = Camera.main;
        inputManager = GetComponent<PlayerInputManager>();
        cameraScript = mainCamera.GetComponent<CameraController>();
        winText = winTextObj.GetComponent<TextMeshProUGUI>();

        playerSpawns = GetChildren();
    }

    private List<Transform> GetChildren()
    {
        List<Transform> spawns = new List<Transform>();

        foreach (Transform child in transform)
        {
            spawns.Add(child);
        }
        return spawns;
    }

    private void Start()
    {
        // Make a Randomly Generated BG of Stars.
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

        SpawnPlayers();
    }

    private void SpawnPlayers()
    {
        foreach (var player in PlayerConfigManager.playerControllers)
        {
            InputDevice controller = PlayerConfigManager.playerControllers[player.Key];

            PlayerInput playerInput = PlayerInput.Instantiate(playerPrefab, player.Key, "Gamepad", -1, controller);

            GameObject playerBody = playerInput.gameObject.transform.GetChild(0).gameObject;
            SpriteRenderer playerSprite = playerBody.GetComponent<SpriteRenderer>();
            PlayerMovement playerMovement = playerBody.GetComponent<PlayerMovement>();

            Color color = PlayerConfigManager.playerColors[player.Key];

            playerInput.gameObject.transform.position = playerSpawns[player.Key].position;
            playerSprite.color = color;

            // Set player script variables
            playerMovement.gameScript = this;
            playerMovement.pauseTint = pauseTint;
            playerMovement.theme = color;

            playerInput.currentActionMap = playerInput.actions.FindActionMap("Player");

            string objName = "Player " + (player.Key + 1).ToString();
            playerInput.name = objName;
            playerBody.name = objName + " Body";
            cameraScript.addToCam(playerBody);
        }
    }

    public void Win(GameObject winner)
    {
        PlayerMovement winnersScript = winner.GetComponent<PlayerMovement>();
        winnersScript.isWinner = true;

        winTextObj.SetActive(true);
        winText.color = winnersScript.theme;
        winText.text = winner.transform.parent.gameObject.name + " wins!";
        gameEnded = true;
    }
}