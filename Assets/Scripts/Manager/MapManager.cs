using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public interface ICullable {
    float PositionZ { get; }
    void Cull();
}

public class MapManager : MonoBehaviour
{
    [SerializeField] private GameObject segmentPrefab;
    [SerializeField] private GameObject pedPrefab;
    [SerializeField] private Transform segmentParent;
    [SerializeField] private Transform pedParent;

    private const int SEGMENT_AMOUNT = 15;
    private const float SEGMENT_LENGTH = 10;

    public int PedSpawnAmount { get; set; } = 3;
    public float PedSpawnPerDist { get; set; } = 15;

    private List<SegmentController> segments;
    private List<PedestrianController> peds;
    private List<ICullable> cullables;
    private IGameContext ctx => GameContext.instance;
    private float lastSegmentZ = -SEGMENT_LENGTH;
    private float distancePedCounter;
    private SegmentParameters loadedSegmentParameters;

    private void Awake()
    {
        cullables = new List<ICullable>();
        segments = new List<SegmentController>();
        peds = new List<PedestrianController>();

        for (int i = 0; i < SEGMENT_AMOUNT; i++)
        {
            AppendNewSegment();
        }

        StartCoroutine(CullableCO());
    }

    private void FixedUpdate()
    {
        distancePedCounter += ctx.PlayerController.ZDistanceTravelledLastTick;
        if (distancePedCounter > PedSpawnPerDist)
        {
            SpawnPedestrianBatch(PedSpawnAmount);
            distancePedCounter -= PedSpawnPerDist;
        }

        if (ctx.PlayerController.transform.position.z > lastSegmentZ - SEGMENT_LENGTH * (SEGMENT_AMOUNT - 2))
        {
            AppendFirstSegment();
        }
    }

    public void LoadSegmentParameters(SegmentParameters parameters)
    {
        loadedSegmentParameters = parameters;
    }

    private void AppendNewSegment()
    {
        var segment = Instantiate(segmentPrefab, segmentParent).GetComponent<SegmentController>();
        AppendSegment(segment);
    }

    private void AppendFirstSegment()
    {
        var segment = segments[0];
        segments.RemoveAt(0);
        AppendSegment(segment);
    }

    private void AppendSegment(SegmentController segment)
    {
        segment.LoadParameters(loadedSegmentParameters);

        if (segments.Count > 0) segment.AlignToSegment(segments.Last());
        else segment.transform.position = Vector3.zero;

        segments.Add(segment);
        lastSegmentZ = segment.transform.position.z;
    }

    private void SpawnPedestrianBatch(int amount = 3)
    {
        var lastSegment = segments.Last();
        var spawnpoints = lastSegment.GetSpawnpoints(amount);
        for (int i = 0; i < amount; i++)
        {
            var ped = SpawnPedestrian();
            ped.transform.position = spawnpoints[i];
        }
    }

    private PedestrianController SpawnPedestrian()
    {
        var ped = Instantiate(pedPrefab, pedParent).GetComponent<PedestrianController>();
        peds.Add(ped);
        return ped;
    }


    private IEnumerator CullableCO()
    {
        while (true)
        {
            var storedCullables = cullables.ToList();
            var yieldIndex = 0;
            foreach (var cullable in storedCullables)
            {
                if (cullable == null)
                    continue;

                if (ctx.PlayerController.transform.position.z > cullable.PositionZ + 10f)
                {
                    cullable.Cull();
                }
                yieldIndex++;

                if (yieldIndex > 5)
                {
                    yield return new WaitForSeconds(0.06f);
                    yieldIndex = 0;
                }
            }

            yield return new WaitForSeconds(0.5f);
        }
    }
}
