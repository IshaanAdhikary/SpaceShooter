using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class GamepadJoinBehavior : MonoBehaviour
{
    public GameObject playerMenu;

    public int numberOfActivePlayers { get; private set; } = 0;

    private GameObject rootMenu;

    private void Awake()
    {
        rootMenu = GameObject.Find("MainLayout");
    }

    // Start is called before the first frame update
    void Start()
    {
        // This subscribes us to events that will fire if any button is pressed.  We'll most certainly want to throw this away 
        // when not in a selection screen (performance intensive)!
        var myAction = new InputAction(binding: "/*/<button>");
        myAction.performed += (action) =>
        {
            //UnityEngine.Debug.Log(Gamepad.current.description.deviceClass);
            //UnityEngine.Debug.Log(action.control.device.description);
            AddPlayer(action.control.device);
        };
        myAction.Enable();
    }

    void AddPlayer(InputDevice device)
    {
        // Avoid running if the device is already paired to a player
        foreach (var player in PlayerInput.all)
        {
            foreach (var playerDevice in player.devices)
            {
                if (device == playerDevice)
                {
                    return;
                }
            }
        }

        //UnityEngine.Debug.Log(device.device);


        // Don't execute if not a gamepad or joystick
        if (!device.displayName.Contains("Controller") && !device.displayName.Contains("Gamepad"))
            return;

        if (!playerMenu.activeInHierarchy)
        {
            PlayerInput theMenu = PlayerInput.Instantiate(playerMenu, -1, "Gamepad", -1, device);

            theMenu.gameObject.GetComponent<PlayerSetupMenuController>().SetPlayerIndex(theMenu.playerIndex);
            theMenu.gameObject.transform.SetParent(rootMenu.transform);
        }

    }
}
