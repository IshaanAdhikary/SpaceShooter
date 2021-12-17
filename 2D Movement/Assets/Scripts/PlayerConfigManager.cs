using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class PlayerConfigManager : MonoBehaviour
{
    private List<PlayerConfiguration> playerConfigs;

    [SerializeField]
    private int MaxPlayers = 4;

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
        }
    }

    public void SetPlayerColor(int index, Color color)
    {
        playerConfigs[index].PlayerColor = color;
    }

    public void ReadyPlayer(int index)
    {
        playerConfigs[index].isReady = true;
        if (playerConfigs.Count >= 2 && playerConfigs.All(p => p.isReady))
        {
            SceneManager.LoadScene("MainGame");
        }
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
