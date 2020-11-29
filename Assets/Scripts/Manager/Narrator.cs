using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class Narrator : MonoBehaviour
{
    public abstract class Query : ScriptableObject
    {
        [SerializeField] private float minimumZ;

        public float MinimumZ => minimumZ;
        public bool FinishedExecuting => finishedExecuting;
        
        protected bool finishedExecuting;

        public abstract void Execute();
    }

    private int currentQueryIndex = 0;
    private PlayerController playerController;
    private Query lastPlayedQuery;
    private Query[] queries;

    private void Awake()
    {
        playerController = GameContext.instance.PlayerController;

        queries = Resources.LoadAll<Query>("Data/NarrationAutoload").OrderBy(q => q.MinimumZ).ToArray();
        
        HandleQuery();
    }

    private void FixedUpdate()
    {
        HandleQuery();
    }

    private void HandleQuery()
    {
        if (queries.Length <= currentQueryIndex)
            return;
        
        if (lastPlayedQuery != null && !lastPlayedQuery.FinishedExecuting)
            return;

        var query = queries[currentQueryIndex];
        if (query.MinimumZ > playerController.transform.position.z)
            return;

        print("Narrator query played: " + currentQueryIndex);
        query.Execute();
        lastPlayedQuery = query;

        currentQueryIndex++;
    }
}
