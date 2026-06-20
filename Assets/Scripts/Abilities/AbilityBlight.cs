using System.Collections.Generic;
using UnityEngine;

public class AbilityBlight : BaseAbility
{
    [Header("Settings")]
    [SerializeField] private float tickInterval;
    [SerializeField] private float damage;
    [SerializeField] private float radius;
    [SerializeField] private float damageIncreasePerLevel;
    [SerializeField] private float tickReductionPerLevel;


    private float tickTimer = 0f;
    private HashSet<Enemy> enemiesInRange = new HashSet<Enemy>();
    private SpriteRenderer spriteRenderer;

    public override void Activate()
    {
        spriteRenderer = GetComponentInChildren<SpriteRenderer>();
    }

    public override void Upgrade(GameObject prefab)
    {
        if (currentLevel >= maxLevel) return;
        currentLevel++;
        damage += damageIncreasePerLevel;
        tickInterval = Mathf.Max(0.2f, tickInterval - tickReductionPerLevel);
        gameObject.transform.localScale += new Vector3(0.1f, 0.1f, 0f);
    }

    protected override void Update()
    {
        base.Update();
        tickTimer += Time.deltaTime;

        if (tickTimer >= tickInterval)
        {
            foreach (Enemy enemy in new List<Enemy>(enemiesInRange))
            {
                if (enemy != null && enemy.gameObject.activeSelf)
                    enemy.TakeDamage(damage);
            }
            tickTimer = 0f;
        }

        float t = (Mathf.Sin(Time.time * 2f) + 1f) / 2f;
        float alpha = Mathf.Lerp(0.3f, 0.8f, t);
        spriteRenderer.color = new Color(1, 1, 1, alpha);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Enemy"))
            enemiesInRange.Add(collision.GetComponent<Enemy>());
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Enemy"))
            enemiesInRange.Remove(collision.GetComponent<Enemy>());
    }
}
