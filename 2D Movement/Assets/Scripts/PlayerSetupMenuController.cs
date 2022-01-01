using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PlayerSetupMenuController : MonoBehaviour
{
    private int PlayerIndex;

    [SerializeField]
    private TextMeshProUGUI titleText;
    public GameObject readyPanel;
    public Button readyButton;
    public Image colorPreview;

    //private Color[] colorsIndex = new Color[4] {new Color(0.5f, 1, 0.87f), new Color(1, 0.5f, 0.5f), new Color(1, 1, 0.5f), new Color(0.5f, 1, 0.5f) };
    // ROYGBP
    public Color[] colorsIndex = new Color[6] { new Color(1, 0.5f, 0.5f), new Color(1, 0.8f , 0.4f), new Color(1, 1, 0.5f), new Color(0.5f, 1, 0.5f), new Color(0.5f, 1, 0.87f), new Color(0.85f, 0.5f, 0.85f) };
    public int currentColorIndex = 0;

    private float ignoreInputTime = 0.25f;
    private bool inputEnabled;
    /* delete this later */ private bool isReady;

    private void Start()
    {
        colorPreview.color = colorsIndex[currentColorIndex];
    }

    public void SetPlayerIndex(int pi)
    {
        PlayerIndex = pi;
        titleText.SetText("Player " + (pi + 1).ToString());
        ignoreInputTime = Time.time + ignoreInputTime;
    }

    // Update is called once per frame
    void Update()
    {
        if (Time.time > ignoreInputTime)
        {
            inputEnabled = true;
        }
    }

    public void SwitchColor(bool isRight)
    {
        switch (isRight)
        {
            case true:
                currentColorIndex++;
                break;
            case false:
                currentColorIndex--;
                break;
        }
        currentColorIndex = (currentColorIndex + colorsIndex.Length) % colorsIndex.Length;
        colorPreview.color = colorsIndex[currentColorIndex];
    }

    public void ReadyPlayer()
    {
        if (!inputEnabled) { return; }

        // Delete later
        isReady = true;

        PlayerConfigManager.Instance.ReadyPlayer(PlayerIndex);
        readyButton.gameObject.SetActive(false);
    }

    public void UnReadyPlayer()
    {
        if (!inputEnabled || !isReady) { return; }

        PlayerConfigManager.Instance.UnReadyPlayer(PlayerIndex);
        readyButton.gameObject.SetActive(true);
    }
}
