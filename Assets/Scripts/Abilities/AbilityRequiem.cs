using UnityEngine;
using UnityEngine.Pool;

public class AbilityRequiem : BaseAbility
{
    [Header("Settings")]
    [SerializeField] float projectileSpeed;
    [SerializeField] float damage;
    [SerializeField] float cooldown;
    [SerializeField] float cooldownReductionPerLevel;
    [SerializeField] float damageIncreasePerLevel;

    [Header("References")]
    [SerializeField] GameObject projectilePrefab;

    private float timer;
    private int waveCount = 1;
    private ObjectPool<RequiemProjectile> pool;

    public override void Initialize(Player p)
    {
        base.Initialize(p);
    }

    public override void Activate()
    {
        pool = new ObjectPool<RequiemProjectile>(
            createFunc: () => Instantiate(projectilePrefab).GetComponent<RequiemProjectile>(),
            actionOnGet: (b) => b.gameObject.SetActive(true),
            actionOnRelease: (b) => b.gameObject.SetActive(false),
            defaultCapacity: 20
        );
    }

    public override void Upgrade(GameObject prefab)
    {
        if (currentLevel >= maxLevel) return;
        currentLevel++;
        cooldown -= cooldownReductionPerLevel;
        damage += damageIncreasePerLevel;

        waveCount = currentLevel < 5 ? currentLevel : 8;
    }

    protected override void Update()
    {
        base.Update();
        timer += Time.deltaTime;

        if (timer >= cooldown)
        {
            FireWaves();
            timer = 0f;
        }
    }

    private void FireWaves()
    {
        Vector2 baseDirection = GetBaseDirection();

        float angleStep = 360f / waveCount;

        for (int i = 0; i < waveCount; i++)
        {
            float angle = i * angleStep;
            Vector2 dir = RotateVector(baseDirection, angle);
            FireProjectile(dir);
        }
    }

    private void FireProjectile(Vector2 direction)
    {
        RequiemProjectile projectile = pool.Get();
        projectile.transform.position = player.transform.position;
        projectile.transform.right = direction;
        projectile.Init(direction, projectileSpeed, (int)damage, pool);
    }

    private Vector2 GetBaseDirection()
    {
        Enemy nearest = player.enemyManager.GetNearestEnemy(player.transform.position);
        if (nearest != null)
            return ((Vector2)nearest.transform.position - (Vector2)player.transform.position).normalized;

        // Enemy yoksa sağa doğru fırlat
        return Vector2.right;
    }

    private Vector2 RotateVector(Vector2 v, float degrees)
    {
        float rad = degrees * Mathf.Deg2Rad;
        float cos = Mathf.Cos(rad);
        float sin = Mathf.Sin(rad);
        return new Vector2(cos * v.x - sin * v.y, sin * v.x + cos * v.y);
    }
}
