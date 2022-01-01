using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.UI;
using UnityEngine.SceneManagement;

public class PlayerConfigManager : MonoBehaviour
{
    private PlayerInputManager manager;
    private List<PlayerConfiguration> playerConfigs;

    public static Dictionary<int, InputDevice> playerControllers = new Dictionary<int, InputDevice>();
    public static Dictionary<int, Color> playerColors = new Dictionary<int, Color>();
    public static GameObject[] playerMenus;
    public static PlayerConfigManager Instance { get; private set; }

    private void Awake()
    {
        if (Instance != null)
        {
            Debug.Log("SINGLETON - Trying to create another instance");
        }
        else
        {
            Instance = this;
            DontDestroyOnLoad(Instance);
            playerConfigs = new List<PlayerConfiguration>();
            manager = GetComponent<PlayerInputManager>();
        }
    }

    public void ReadyPlayer(int index)
    {
        playerConfigs[index].isReady = true;

        // If all players are ready, load next scene.
        if (playerConfigs.Count >= 2 && playerConfigs.All(p => p.isReady))
        {
            playerMenus = GameObject.FindGameObjectsWithTag("PlayerMenuSelect");

            playerControllers.Clear();

            for (int i = 0; i < playerMenus.Length; i++)
            {
                PlayerInput playerInputComponent = playerMenus[i].GetComponent<PlayerInput>();
                PlayerSetupMenuController scrip = playerMenus[i].GetComponent<PlayerSetupMenuController>();

                int playerIndex = playerInputComponent.playerIndex;

                playerControllers.Add(playerIndex, playerInputComponent.devices[0]);
                playerColors.Add(playerIndex, scrip.colorsIndex[scrip.currentColorIndex]);
            }

            manager.enabled = false;
            SceneManager.LoadScene("MainGame");
        }
    }

    public void UnReadyPlayer(int index)
    {
        playerConfigs[index].isReady = false;
    }

    public void HandlePlayerJoin(PlayerInput pi)
    {
        Debug.Log("Player Joined " + pi.playerIndex);
        pi.transform.SetParent(transform);
        if(!playerConfigs.Any(p => p.PlayerIndex == pi.playerIndex))
        {
            playerConfigs.Add(new PlayerConfiguration(pi));
        }
    }
}

public class PlayerConfiguration
{
    public PlayerConfiguration(PlayerInput pi)
    {
        PlayerIndex = pi.playerIndex;
        Input = pi;
    }

    public PlayerInput Input { get; set; }

    public int PlayerIndex { get; set; }

    public bool isReady { get; set; }

    public Color PlayerColor { get; set; }
}
