using System.Collections.Generic;
using UnityEngine;

public class EnemyPool
{
    private Enemy prefab;
    private Queue<Enemy> pool = new Queue<Enemy>();
    private Transform container;

    public EnemyPool(Enemy prefab, int initialSize, Transform container)
    {
        this.prefab = prefab;
        this.container = container;

        for (int i = 0; i < initialSize; i++)
            CreateNew();
    }

    private Enemy CreateNew()
    {
        Enemy enemy = GameObject.Instantiate(prefab, container);
        enemy.gameObject.SetActive(false);
        pool.Enqueue(enemy);
        return enemy;
    }

    public Enemy Get()
    {
        if (pool.Count > 0)
            return pool.Dequeue();

        CreateNew();
        return pool.Dequeue();

    }

    public void Return(Enemy enemy)
    {
        enemy.gameObject.SetActive(false);
        pool.Enqueue(enemy);
    }
}