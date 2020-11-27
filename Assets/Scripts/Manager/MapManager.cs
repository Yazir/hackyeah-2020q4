using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapManager : MonoBehaviour
{
    [SerializeField] private GameObject segmentPrefab;
    [SerializeField] private Transform segmentParent;
    
    private List<Transform> segments;
    private GameContext ctx => GameContext.instance;
    private float lastSegmentZ = -10f;

    private void Awake()
    {
        segments = new List<Transform>();
        for (int i = 0; i < 5; i++)
        {
            AppendNewSegment();
        }
    }

    private void FixedUpdate()
    {
        // if (ctx.PlayerController.transform) {
            
        // }
    }

    private void AppendNewSegment() {
        AppendSegment(Instantiate(segmentPrefab, segmentParent));
    }

    private void AppendSegment(GameObject segment)
    {
        var newPos = new Vector3(0, 0, lastSegmentZ + 9.99f); // 1 segment is 10 units in depth
        segment.transform.position = newPos; 
        lastSegmentZ = segment.transform.position.z;
    }


}
