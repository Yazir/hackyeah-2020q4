using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface ISegmentParameters
{
    float Angle { get; }
    float AngleDeviation { get; }
    bool Forest { get; }
    Material FloorMaterial { get; }
}

[CreateAssetMenu(menuName = "HY/Segment Parameters")]
public class SegmentParameters : ScriptableObject, ISegmentParameters
{
    [SerializeField] private float angle;
    [SerializeField] private float angleRandomness;
    [SerializeField] private bool forest;
    [SerializeField] private Material floorMaterial;
    [SerializeField] private ObstacleSet obstacleSet;

    public float Angle => angle;
    public float AngleDeviation => angleRandomness;
    public bool Forest => forest;
    public Material FloorMaterial => floorMaterial;
    public ObstacleSet ObstacleSet => obstacleSet;
}
