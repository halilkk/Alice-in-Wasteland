using System.Collections.Generic;
using UnityEngine;

public class AbilityManager
{
    private Player player;
    // Track one 'Master' instance per ability type to handle upgrades
    public List<BaseAbility> activeAbilities = new List<BaseAbility>();

    public AbilityManager(Player player)
    {
        this.player = player;
    }

    public void AddAbility(GameObject abilityPrefab)
    {
        if (abilityPrefab == null) return;

        // Get the type of the ability to check for duplicates
        System.Type abilityType = abilityPrefab.GetComponent<BaseAbility>().GetType();
        BaseAbility existing = activeAbilities.Find(a => a.GetType() == abilityType);

        if (existing != null)
        {
            // The existing ability instance handles its own upgrade logic
            existing.Upgrade(abilityPrefab);
        }
        else
        {
            // Create the very first instance of this ability
            GameObject obj = Object.Instantiate(abilityPrefab, player.transform);
            BaseAbility newAbility = obj.GetComponent<BaseAbility>();
            newAbility.Initialize(player);
            
            // Store this instance as the 'Master' for future upgrades
            activeAbilities.Add(newAbility);
        }
    }
}