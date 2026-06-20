using System.Collections.Generic;
using UnityEngine;

public class EnemyManager : MonoBehaviour
{
    [SerializeField] private List<Enemy> enemyPrefabs;
    [SerializeField] private int poolSizePerType;
    [SerializeField] private float spawnInterval;
    [SerializeField] private float spawnIntervalReduction;
    [SerializeField] private Transform container;
    [SerializeField] private OptimizationSettings settings;

    private Dictionary<Enemy, EnemyPool> pools = new Dictionary<Enemy, EnemyPool>();
    private List<Enemy> activeEnemies = new List<Enemy>();
    private Transform playerTransform;
    private float spawnTimer;
    private float gameTime;
    public float GameTime => gameTime;
    private UniformGrid grid;

    private int sliceFrame = 0;
    private const int sliceCount = 32;
    private float lastSpeedUpTime = 0f;

    Player player;

    private void Awake()
    {
        foreach (var prefab in enemyPrefabs)
            pools[prefab] = new EnemyPool(prefab, poolSizePerType, container);

        grid = new UniformGrid(5f);

        Application.targetFrameRate = settings.useGreenComputing ? settings.targetFrameRate : -1;

        player = FindFirstObjectByType<Player>();
        playerTransform = player.transform;
    }

    private void FixedUpdate()
    {
        // Spawn timer
        spawnTimer += Time.deltaTime;
        gameTime += Time.deltaTime;
        if (spawnTimer >= spawnInterval)
        {
            SpawnEnemy();
            spawnTimer = 0f;
        }

        if (settings.useSpatialPartitioning)
            grid.Clear();

        float dt = settings.useTimeSlicing ? Time.deltaTime * sliceCount : Time.deltaTime;

        for (int i = activeEnemies.Count - 1; i >= 0; i--)
        {
            if (activeEnemies[i] != null && activeEnemies[i].gameObject.activeSelf)
            {
                bool shouldTick = !settings.useTimeSlicing || (i % sliceCount == sliceFrame);
                if (shouldTick)
                    activeEnemies[i].Tick(dt);
        
                if (settings.useSpatialPartitioning)
                    grid.Add(activeEnemies[i]);
            }
            else
            {
                activeEnemies.RemoveAt(i);
            }
        }

        if (settings.useTimeSlicing)
            sliceFrame = (sliceFrame + 1) % sliceCount;

        if (gameTime - lastSpeedUpTime >= 30f)
        {
            lastSpeedUpTime = gameTime;
            spawnInterval = Mathf.Max(0.1f, spawnInterval - spawnIntervalReduction);
        }
    }

    private void SpawnEnemy()
    {
        Enemy selectedPrefab = GetWeightedRandom();
        if (selectedPrefab == null) return;

        Enemy enemy;
        if (settings.useObjectPooling)
        {
            EnemyPool pool = pools[selectedPrefab];
            enemy = pool.Get();
            enemy.Initialize(playerTransform, player, this, pool);
        }
        else
        {
            enemy = Instantiate(selectedPrefab);
            enemy.Initialize(playerTransform, player, this, null);
        }

        enemy.transform.position = GetSpawnPosition();
        activeEnemies.Add(enemy);
    }

    public void ReturnEnemy(Enemy enemy, EnemyPool pool)
    {
        if (settings.useObjectPooling && pool != null)
            pool.Return(enemy);
        else
        {
            enemy.gameObject.SetActive(false);
            Destroy(enemy.gameObject);
        }
    }

    private Enemy GetWeightedRandom()
    {
        List<Enemy> available = new List<Enemy>();
        float total = 0f;

        foreach (var prefab in enemyPrefabs)
        {
            if (prefab.data.unlockTime <= gameTime)
            {
                available.Add(prefab);
                total += prefab.data.spawnWeight;
            }
        }

        if (available.Count == 0) return null;

        float random = Random.Range(0f, total);
        float cumulative = 0f;

        foreach (var prefab in available)
        {
            cumulative += prefab.data.spawnWeight;
            if (random <= cumulative) return prefab;
        }

        return available[available.Count - 1];
    }

    private Vector2 GetSpawnPosition()
    {
        Vector2 randomDir = Random.insideUnitCircle.normalized;
        return (Vector2)playerTransform.position + randomDir * 15f;
    }

    public Enemy GetNearestEnemy(Vector2 position)
    {
        if (settings.useSpatialPartitioning)
            return grid.GetNearest(position);
        else
            return GetNearestEnemyOld(position);
    }

    public Enemy GetNearestEnemyOld(Vector2 position)
    {
        Enemy nearest = null;
        float nearestDistance = float.MaxValue;

        for (int i = 0; i < activeEnemies.Count; i++)
        {
            float distance = Vector2.Distance(position, activeEnemies[i].transform.position);
            if (distance < nearestDistance)
            {
                nearestDistance = distance;
                nearest = activeEnemies[i];
            }
        }
        if (nearestDistance > 10f) return null;
        return nearest;
    }

    public Enemy GetRandomEnemy(Vector2 position)
    {
        List<Enemy> nearbyEnemies = new List<Enemy>();

        for (int i = 0; i < activeEnemies.Count; i++)
        {
            if (Vector2.Distance(position, activeEnemies[i].transform.position) <= 7f)
                nearbyEnemies.Add(activeEnemies[i]);
        }

        if (nearbyEnemies.Count == 0) return null;
        return nearbyEnemies[Random.Range(0, nearbyEnemies.Count)];
    }
}
