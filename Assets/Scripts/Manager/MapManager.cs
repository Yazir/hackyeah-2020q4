using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class MapManager : MonoBehaviour
{
    [SerializeField] private GameObject segmentPrefab;
    [SerializeField] private Transform segmentParent;

    private const int SEGMENT_AMOUNT = 50;
    private const float SEGMENT_LENGTH = 10;
    
    private List<SegmentController> segments;
    private GameContext ctx => GameContext.instance;
    private float lastSegmentZ = -10f;

    private void Awake()
    {
        segments = new List<SegmentController>();
        for (int i = 0; i < SEGMENT_AMOUNT; i++)
        {
            AppendNewSegment();
        }
    }

    private void FixedUpdate()
    {
        if (ctx.PlayerController.transform.position.z > lastSegmentZ - SEGMENT_LENGTH*(SEGMENT_AMOUNT-2)) {
            AppendFirstSegment();
        }
    }

    private void AppendNewSegment() {
        var segment = Instantiate(segmentPrefab, segmentParent).GetComponent<SegmentController>();
        AppendSegment(segment);
    }

    private void AppendFirstSegment() {
        var segment = segments[0];
        segments.RemoveAt(0);
        AppendSegment(segment);
    }

    private void AppendSegment(SegmentController segment)
    {
        if (segments.Count > 0) segment.AlignToSegment(segments.Last());
        else {
            segment.transform.position = Vector3.zero;
        }
        segments.Add(segment);
        lastSegmentZ = segment.transform.position.z;
    }


}
