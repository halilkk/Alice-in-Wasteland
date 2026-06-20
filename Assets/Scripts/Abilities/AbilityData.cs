using UnityEngine;

[CreateAssetMenu(menuName = "Game/Ability Data")]
public class AbilityData : ScriptableObject
{
    public string abilityName;
    public Sprite icon;
    public GameObject prefab;
    public float chanceMultiplier;
}
