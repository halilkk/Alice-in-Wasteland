 using System.Collections.Generic;
using UnityEngine;

public class AbilityOmen : BaseAbility
{
    [Header("Settings")]
    [SerializeField] private float rotationSpeed; 
    [SerializeField] private float radius;
    [SerializeField] float damage;

    // Shared list for all active swords to handle positioning and sync
    private static List<AbilityOmen> allSwords = new List<AbilityOmen>();

    public override void Initialize(Player p)
    {
        // When a new sword is created, sync it with the group's current stats
        if (allSwords.Count > 0)
        {
            this.currentLevel = allSwords[0].currentLevel;
            this.rotationSpeed = allSwords[0].rotationSpeed;
        }
        base.Initialize(p);
    }

    public override void Activate()
    {
        if (!allSwords.Contains(this))
        {
            allSwords.Add(this);
        }

        RepositionAbilities();
    }
    
    protected override void Update()
    {
        
        // Handle orbiting movement
        if (player != null)
        {
            transform.RotateAround(player.transform.position, Vector3.forward, rotationSpeed * Time.deltaTime);
            transform.Rotate(0, 0, -60f * Time.deltaTime);  // Buraya projectile kendi ekseninde donme ekle bu konumda duzgun calismiyor
        }
    }

    public override void Upgrade(GameObject prefab)
    {
        if (currentLevel >= maxLevel) return;
        
        currentLevel++;

        // SWORD-SPECIFIC UPGRADE: Each upgrade spawns a new physical sword instance
        GameObject nextSword = Object.Instantiate(prefab, player.transform);
        nextSword.GetComponent<BaseAbility>().Initialize(player);

        // Update stats (like speed) for ALL currently active swords
        foreach (var sword in allSwords)
        {
            sword.rotationSpeed += 10f;
            sword.damage += 3f;
            sword.currentLevel = this.currentLevel;
        }
        
    }

    private void RepositionAbilities()
    {
        allSwords.RemoveAll(s => s == null);
        int swordCount = allSwords.Count;
        if (swordCount == 0) return;

        float angleStep = 360f / swordCount;

        for (int i = 0; i < swordCount; i++)
        {
            float angleRad = i * angleStep * Mathf.Deg2Rad;
            Vector3 offset = new Vector3(Mathf.Cos(angleRad), Mathf.Sin(angleRad), 0) * radius;
            
            // Position the sword and make it face outwards from the player
            allSwords[i].transform.position = player.transform.position + offset;
            allSwords[i].transform.right = offset.normalized;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Enemy"))
        {
            collision.GetComponent<Enemy>().TakeDamage(damage);
        }   
    }

    private void OnDestroy()
    {
        allSwords.Remove(this);
        RepositionAbilities();
    }
}