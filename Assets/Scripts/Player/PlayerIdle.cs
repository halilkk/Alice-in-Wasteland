using UnityEngine;

public class PlayerIdle
{
    float speed = 1f;
    float amplitude = 0.2f;

    Player player;
    public PlayerIdle(Player player)
    {
        this.player = player;
        
    }
    public void Update()
    {
        Idle();
    }

    void Idle()
    {
        float y = Mathf.Sin(Time.time * speed) * amplitude;
        player.PlayerSprite.transform.localPosition = new Vector2(0, y);
    }
}