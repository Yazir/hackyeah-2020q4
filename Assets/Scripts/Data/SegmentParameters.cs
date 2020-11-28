using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface ISegmentParameters
{
    GameObject[] WallSegments { get; }
    float Angle { get; }
    float AngleDeviation { get; }
}

[CreateAssetMenu(menuName = "HY/Segment Parameters")]
public class SegmentParameters : ScriptableObject, ISegmentParameters
{
    [SerializeField] private GameObject[] wallSegments;
    [SerializeField] private float angle;
    [SerializeField] private float angleRandomness;

    public GameObject[] WallSegments => wallSegments;
    public float Angle => angle;
    public float AngleDeviation => angleRandomness;

    // public SegmentParameters(GameObject[] wallSegments, float angle)
    // {
    //     this.wallSegments = wallSegments;
    //     this.angle = angle;
    // }
}
