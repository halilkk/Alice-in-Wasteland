using System;
using UnityEngine;

public class PlayerHealthController
{
    Player player;
    public float currentHealth;
    public float maxhealth;
    GameObject GameOverScreen;
    // DARK SHIELD REFERANSINI DUZELT

    public Action<float,float> OnHealthChanged;
    // using action to update health bar UI when health changes, instead of using events and delegates for simplicity
    public PlayerHealthController(Player player)
    {
        this.player = player;
        this.GameOverScreen = player.GameOverScreen;
        currentHealth = player.playerStats.health;
        maxhealth = player.playerStats.maxHealth;
    }

    public bool isGodMode = false;

    public void TakeDamage(int damage)
    {
        if (isGodMode)  return;

        AbilityDarkShield shield = player.abilityManager.activeAbilities.Find(a => a is AbilityDarkShield) as AbilityDarkShield; // Check if the player has the Dark Shield ability
        if(shield != null && shield.isShieldActive)
        {
            shield.ShieldTrigger();
            return;
        }

        AudioManager.Instance.Play("PlayerHit");
        ScreenShake();
        currentHealth -= damage;
        OnHealthChanged?.Invoke(currentHealth, maxhealth);
        
        if(currentHealth <= 0)
        {
            AudioManager.Instance.Play("PlayerDie");
            Die();
        }
    }

    // detect collision with enemy and reduce health, if health is 0 or less, call Die method
    public void Die()
    {
        Time.timeScale = 0;
        GameOverScreen.SetActive(true);
    }   

    void ScreenShake()
    {
        player.impulseSource.GenerateImpulseWithForce(0.3f);
    }

    public void GodModeToggle()
    {
        isGodMode = !isGodMode;
        player.GodMode.gameObject?.SetActive(isGodMode);
    }
}