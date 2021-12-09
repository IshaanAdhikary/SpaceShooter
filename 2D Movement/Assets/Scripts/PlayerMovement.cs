using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    public float moveSpeed = 0;

    private CharacterController2D controller;
    private Vector2 inputValue = Vector2.zero;

    void Awake()
    {
        controller = GetComponent<CharacterController2D>();
    }

    private void FixedUpdate()
    {
        controller.Move(inputValue);
    }

    public void Move(InputAction.CallbackContext context)
    {
        inputValue = context.ReadValue<Vector2>().normalized * moveSpeed;
    }
}
