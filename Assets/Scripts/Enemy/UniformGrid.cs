using System.Collections.Generic;
using UnityEngine;

public class UniformGrid
{
    private float cellSize;
    private Dictionary<(int, int), List<Enemy>> grid = new Dictionary<(int, int), List<Enemy>>();

    public UniformGrid(float cellSize)
    {
        this.cellSize = cellSize;
    }

    private (int, int) GetCell(Vector2 position)
    {
        int x = Mathf.FloorToInt(position.x / cellSize);
        int y = Mathf.FloorToInt(position.y / cellSize);
        return (x, y);
    }

    // Clear lists but reuse them to avoid GC allocations
    public void Clear()
    {
        foreach (var list in grid.Values)
            list.Clear();
    }

    // Add enemy to grid
    public void Add(Enemy enemy)
    {
        (int, int) cell = GetCell(enemy.transform.position);

        if (!grid.TryGetValue(cell, out List<Enemy> list))
        {
            list = new List<Enemy>();
            grid[cell] = list;
        }

        list.Add(enemy);
    }

    public Enemy GetNearest(Vector2 position)
    {
        (int, int) centerCell = GetCell(position);

        Enemy nearest = null;
        float nearestDistance = float.MaxValue;

        for (int x = -1; x <= 1; x++)
        {
            for (int y = -1; y <= 1; y++)
            {
                (int, int) cell = (centerCell.Item1 + x, centerCell.Item2 + y);

                if (!grid.ContainsKey(cell)) continue;

                foreach (Enemy enemy in grid[cell])
                {
                    float distance = Vector2.Distance(position, enemy.transform.position);
                    if (distance < nearestDistance)
                    {
                        nearestDistance = distance;
                        nearest = enemy;
                    }
                }
            }
        }

        return nearest;
    }
}
