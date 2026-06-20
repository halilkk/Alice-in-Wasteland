using UnityEngine;

public abstract class BaseAbility : MonoBehaviour
{
    protected Player player;

    protected int currentLevel = 1;
    protected int maxLevel = 5;
    public bool IsMaxed => currentLevel >= maxLevel;

    public virtual void Initialize(Player p)
    {
        player = p;
        Activate();
    }

    protected virtual void Update()
    {
        // Base update can be used for shared logic like timers
    }

    public abstract void Activate();

    // Pass the prefab to the upgrade method so the ability can decide if it needs to spawn more instances
    public abstract void Upgrade(GameObject prefab);
}