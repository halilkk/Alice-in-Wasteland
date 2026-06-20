using UnityEngine;

public class PlayerStats
{
    Player player;

    public int health = 30;
    public int maxHealth = 30;   
    public float movementSpeed = 1.8f;

    public PlayerStats(Player player)
    {
        this.player = player;
    }




}