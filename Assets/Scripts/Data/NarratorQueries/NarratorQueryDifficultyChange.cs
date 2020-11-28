using System.Collections;
using System.Collections.Generic;
using UnityEngine;


// [CreateAssetMenu(menuName = "HY/Narrator Query/Difficulty Change")]
public class NarratorQueryDifficultyChange : Narrator.Query
{
    public int pedSpawnAmount = -1;
    public float pedSpawnPerDist = -1;

    public override void Execute()
    {
        var mapManager = GameContext.instance.MapManager;

        if (pedSpawnAmount != -1) mapManager.PedSpawnAmount = pedSpawnAmount;
        if (pedSpawnPerDist != -1) mapManager.PedSpawnPerDist = pedSpawnPerDist;

        finishedExecuting = true;
    }
}