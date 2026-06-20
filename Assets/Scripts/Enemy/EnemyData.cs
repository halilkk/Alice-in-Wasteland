using UnityEngine;

[CreateAssetMenu(fileName = "EnemyData", menuName = "Game/Enemy Data")]
public class EnemyData : ScriptableObject
{
    public string enemyName;
    public float maxHp;
    public float moveSpeed;
    public int damage;
    public float attackRange;
    public int expReward;

    [Header("Spawn Settings")]
    public float spawnWeight;
    public float unlockTime;
}