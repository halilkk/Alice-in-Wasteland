using Unity.VisualScripting;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    Vector2 direction;
    public EnemyData data;
    Player player;

    private int damage;   
    private float currentHp;
    private float moveSpeed;
    private Transform playerTransform;
    private LevelManager levelManager;
    private EnemyManager enemyManager;
    private EnemyPool pool;

    public void Initialize(Transform playerTransform, Player player, EnemyManager enemyManager, EnemyPool pool)
    {
        this.player = player;
        this.playerTransform = playerTransform;
        this.enemyManager = enemyManager;
        this.pool = pool;
        damage = data.damage;
        currentHp = data.maxHp;
        moveSpeed = data.moveSpeed;
        gameObject.SetActive(true);
    }

    public void Tick(float deltaTime)
    {
        MoveTowardsPlayer(deltaTime);
        SetDirection();
    }

    private Vector2 MoveTowardsPlayer(float deltaTime)
    {
        direction = (playerTransform.position - transform.position).normalized;
        transform.position += (Vector3)(direction * moveSpeed * deltaTime);
        return direction;
    }

    public void TakeDamage(float amount)
    {
        AudioManager.Instance.Play("Hit");
        currentHp -= amount;
        if (currentHp <= 0) Die();
    }
    private void SetDirection()
    {
        if(direction.x > 0)
        {
            transform.localScale = new Vector2(1,1);
        }
        else if(direction.x < 0)
        {
            transform.localScale = new Vector2(-1,1);
        }
    }

    private void Die()
    {
        player.levelManager.AddExp(data.expReward);
        enemyManager.ReturnEnemy(this, pool);
    }


    private void OnTriggerEnter2D(Collider2D collision)
    {
        // Enemy uses OnTriggerEnter2D to detect collision with player and projectiles, and reacts accordingly
        if (collision.CompareTag("Player"))
        {
            player.playerHealth.TakeDamage(damage);
        }
    }
}