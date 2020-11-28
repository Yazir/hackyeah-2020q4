using System.Collections;
using System.Collections.Generic;
using UnityEngine;


// [CreateAssetMenu(menuName = "HY/Narrator Query / Segment Change")]
public class NarratorQuerySegmentChange : Narrator.Query
{
    [SerializeField] private SegmentParameters parameters;

    public ISegmentParameters Parameters => parameters;

    public override void Execute()
    {
        GameContext.instance.MapManager.LoadSegmentParameters(parameters);
        finishedExecuting = true;
    }
}