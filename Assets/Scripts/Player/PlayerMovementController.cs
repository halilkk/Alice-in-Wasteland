using Unity.VisualScripting;
using UnityEngine;

public class PlayerMovementController
{
    Player player;

    float moveSpeed;

    public PlayerMovementController(Player player)
    {
        this.player = player;
        this.moveSpeed = player.playerStats.movementSpeed;
    }

    public void FixedUpdate()
    {
        Move();
        CheckDirection();
    }


    public void Move()
    {
        player.GetComponent<Rigidbody2D>().linearVelocity = new Vector2(
        player.playerInput.movementInput.x * moveSpeed,
        player.playerInput.movementInput.y * moveSpeed
        );

        Vector3 pos = player.transform.position;
        pos.x = Mathf.Clamp(pos.x, -20, 20);
        pos.y = Mathf.Clamp(pos.y, -9.5f,9.5f);
        player.transform.position = pos;
    }
    private void CheckDirection()
    {
        if (player.playerInput.movementInput.x < 0)
        {
            player.PlayerSprite.transform.localScale = new Vector3(-1, 1, 1);
        }
        else if (player.playerInput.movementInput.x > 0)
        {
            player.PlayerSprite.transform.localScale = new Vector3(1, 1, 1);
        }
    }
}