using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(menuName = "HY/Narrator Query")]
public class NarratorQuery : Narrator.Query
{
    public int pedSpawnAmount = -1;
    public float pedSpawnPerDist = -1;
    public SegmentParameters segmentParameters = null;

    public override void Execute()
    {
        var mapManager = GameContext.instance.MapManager;

        if (pedSpawnAmount != -1) mapManager.PedSpawnAmount = pedSpawnAmount;
        if (pedSpawnPerDist != -1) mapManager.PedSpawnPerDist = pedSpawnPerDist;
        if (segmentParameters != null)  mapManager.LoadSegmentParameters(segmentParameters);

        finishedExecuting = true;
    }
}