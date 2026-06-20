using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Pool;

public class AbilityHex : BaseAbility
{
    [Header("Settings")]
    [SerializeField] float bulletSpeed;
    [SerializeField] float cooldown;
    [SerializeField] float cooldownReductionPerLevel;
    [SerializeField] float damage;
    [SerializeField] float damageIncreasePerLevel;
    [SerializeField] float projectileCount;

    [Header("References")]
    [SerializeField] GameObject bulletPrefab;

    float timer;
    ObjectPool<HexProjectile> bulletPool;
    public override void Initialize(Player p)
    {
        base.Initialize(p);
    }

    public override void Activate()
    {
        bulletPool = new ObjectPool<HexProjectile>(
            createFunc: () => Instantiate(bulletPrefab).GetComponent<HexProjectile>(),
            actionOnGet: (b) => b.gameObject.SetActive(true),
            actionOnRelease: (b) => b.gameObject.SetActive(false),
            defaultCapacity: 10
        );
    }

    public override void Upgrade(GameObject prefab)
    {
        if (currentLevel >= maxLevel) return;
        currentLevel++;

        // IRON BULLET-SPECIFIC UPGRADE: Each upgrade reduces cooldown and increases damage
        cooldown = Mathf.Max(0.2f, cooldown - cooldownReductionPerLevel);
        damage += damageIncreasePerLevel;
    }

    protected override void Update()
    {
        base.Update();
        timer += Time.deltaTime;
        Shoot();
    }

    private void Shoot()
    {
        if (timer < cooldown) return;
        CreateBullet();
        timer = 0f;
    }

    private void CreateBullet()
    {
        Vector2 direction = SetDirection();
        if (direction == Vector2.zero) return; // No valid target

        HexProjectile bullet = bulletPool.Get();
        bullet.transform.position = player.transform.position;
        bullet.transform.right = direction;
        bullet.Init(direction, bulletSpeed,damage, bulletPool);

        AudioManager.Instance.Play("Bullet");
    }

    private Vector2 GetEnemy()
    {         
        Enemy enemy = player.enemyManager.GetNearestEnemy(player.transform.position);
        return enemy != null ? (Vector2)enemy.transform.position : Vector2.zero;
    }

    private Vector2 SetDirection()
    {
        Vector2 targetPos = GetEnemy();
        if (targetPos == Vector2.zero) return Vector2.zero; // No enemies available
        Vector2 direction = (targetPos - (Vector2)player.transform.position).normalized;
        return direction;
    }

}