using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Pool;

public class AbilityWrath : BaseAbility
{
    [Header("Settings")]
    [SerializeField] float cooldown;
    [SerializeField] float cooldownReduction;
    [SerializeField] float explosionRadius;
    [SerializeField] float damage;
    [SerializeField] GameObject ExplodePrefab;

    float timer;
    private int hexCount = 1;

    ObjectPool<WrathProjectile> pool;

    public override void Initialize(Player p)
    {
        base.Initialize(p);
    }

    public override void Activate()
    {
        pool = new ObjectPool<WrathProjectile>
            (
                createFunc: () => Instantiate(ExplodePrefab).GetComponent<WrathProjectile>(),
                actionOnGet: (b) => b.gameObject.SetActive(true),
                actionOnRelease: (b) => b.gameObject.SetActive(false),
                defaultCapacity: 10
            );
    }

    public override void Upgrade(GameObject prefab)
    {
        if (currentLevel >= maxLevel) return;
        currentLevel++;
        cooldown -= cooldownReduction;
        damage += 10f;

        if(currentLevel == 3)
        {
            hexCount = 2;
        }
         else if(currentLevel == 5)
        {
            hexCount = 3;
        }
    }

    protected override void Update()
    {
        
        timer += Time.deltaTime;

        if (timer >= cooldown)
        {
            for(int i = 0; i < hexCount; i++)
            {
                ShootRandom();
            }
            timer = 0;
        }
    }

    Enemy GetRandom()
    {
        return player.enemyManager.GetRandomEnemy(player.transform.position);
    }

    void ShootRandom()
    {
        Enemy enemy =  GetRandom();
        if (enemy == null) return;
        WrathProjectile explosion = pool.Get();
        explosion.transform.position = enemy.transform.position;
        AudioManager.Instance.Play("Explosion");
        explosion.Init(pool);
        enemy.TakeDamage(damage);
    }
}