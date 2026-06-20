using UnityEngine;

[CreateAssetMenu(fileName = "OptimizationSettings", menuName = "Game/Optimization Settings")]
public class OptimizationSettings : ScriptableObject
{
    public bool useObjectPooling = true;
    public bool useSpatialPartitioning = true;
    public bool useTimeSlicing = true;
    public bool useGreenComputing = true;
    public int targetFrameRate;
}
