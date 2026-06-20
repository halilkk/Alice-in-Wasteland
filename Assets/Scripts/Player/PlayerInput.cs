using System;
using UnityEngine;

public class PlayerInput
{
    public Player player;
    public InputPlayer inputPlayer;
    public Vector2 movementInput;

    public Action onPause;

    public PlayerInput(Player player)
    {
        this.player = player;
        inputPlayer = new InputPlayer();

        inputPlayer.Movement.PlayerMove.performed += ctx => movementInput = ctx.ReadValue<Vector2>();
        inputPlayer.Movement.PlayerMove.canceled += ctx => movementInput = Vector2.zero;
        inputPlayer.UI.Pause.performed += ctx => onPause?.Invoke();
        inputPlayer.UI.GodMode.performed += ctx => player.playerHealth.GodModeToggle();

        inputPlayer.Enable();
    }
}