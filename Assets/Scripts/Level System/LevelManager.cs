using System;
using UnityEngine;

public class LevelManager
{
    Player player;

    public int currentLevel = 1;
    public int currentExp = 0;
    public int currentMaxExp = 10;

    public Action onLevelUp;
    public Action onExpChanged;

    public LevelManager(Player player)
    {
        this.player = player;
    }
    public void AddExp(int exp)
    {
        currentExp += exp;
        if (currentExp >= currentMaxExp)
        {
            LevelUp();
        }
        onExpChanged?.Invoke(); // Invoke exp changed event
    }

    void LevelUp()
    {
        currentLevel++;
        currentMaxExp = (int)(currentMaxExp * 1.4f); // Increase the max exp for the next level
        currentExp = 0; // Reset current exp
        onLevelUp?.Invoke(); // Invoke level up event
    }
}