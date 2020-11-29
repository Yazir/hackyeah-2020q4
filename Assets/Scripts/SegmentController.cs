using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class SegmentController : MonoBehaviour
{
    [SerializeField] private Transform corePivot;
    [SerializeField] private Transform coreSocket;
    [SerializeField] private Transform leftWall;
    [SerializeField] private Transform leftWallPivot;
    [SerializeField] private Transform leftWallSocket;
    [SerializeField] private StreetGen leftGen;
    [SerializeField] private Transform rightWall;
    [SerializeField] private Transform rightWallPivot;
    [SerializeField] private Transform rightWallSocket;
    [SerializeField] private StreetGen rightGen;
    [SerializeField] private MeshRenderer floor;
    [SerializeField] private GameObject[] forestObstacles;

    public Transform CoreSocket => coreSocket;
    public Transform LeftWallSocket => leftWallSocket;
    public Transform RightWallSocket => rightWallSocket;
    public ISegmentParameters LoadedParameters => loadedParameters;

    static private float globalAppliedAngleDeviation = 0;

    private ISegmentParameters loadedParameters;
    private float angle = 1;
    private List<GameObject> obstacles;

    private void Awake()
    {
        obstacles = new List<GameObject>();
    }

    public void AlignToSegment(SegmentController otherSegment) {
        var deviationDecay = globalAppliedAngleDeviation*0.65f;
        globalAppliedAngleDeviation -= deviationDecay;
        var outAngle = angle - deviationDecay;

        if (loadedParameters != null)
        {
            var angleDeviation = (Random.value-0.5f) * LoadedParameters.AngleDeviation;
            outAngle += angleDeviation;
            globalAppliedAngleDeviation = angleDeviation;
        }

        var corePivotOffset = corePivot.transform.position - transform.position;
        transform.position = otherSegment.CoreSocket.transform.position - corePivotOffset;

        leftWall.localEulerAngles = new Vector3(0, outAngle, 0);
        var leftWallPivotOffset = leftWallPivot.transform.position - leftWall.transform.position;
        leftWall.transform.position = otherSegment.leftWallSocket.transform.position - leftWallPivotOffset;

        rightWall.localEulerAngles = new Vector3(0, -outAngle, 0);
        var rightWallPivotOffset = rightWallPivot.transform.position - rightWall.transform.position;
        rightWall.transform.position = otherSegment.rightWallSocket.transform.position - rightWallPivotOffset;

        // Wall offset at longer ranges fix
        var leftWallLocalPosition = leftWall.transform.localPosition;
        leftWallLocalPosition.x = Mathf.Lerp(leftWallLocalPosition.x, 0, 0.25f);
        leftWall.transform.localPosition = leftWallLocalPosition;

        var rightWallLocalPosition = rightWall.transform.localPosition;
        rightWallLocalPosition.x = Mathf.Lerp(rightWallLocalPosition.x, 0, 0.25f);
        rightWall.transform.localPosition = rightWallLocalPosition;

        CleanupObstacles();
        if (loadedParameters.Forest)
            GenerateObstacles();
    }

    public void LoadParameters(ISegmentParameters parameters)
    {
        if (loadedParameters == parameters)
            return;
        
        angle = parameters.Angle;
        
        loadedParameters = parameters;

        if (parameters.Forest)
        {
            leftGen.ActivateForest();
            rightGen.ActivateForest();
        }

        floor.material = parameters.FloorMaterial;
    }

    public Vector3[] GetSpawnpoints(int amount = 1) {
        var margin = 3; // Too low = spawnpoints in walls
        var distance = Vector3.Distance(leftWallSocket.transform.position, rightWallSocket.transform.position) - margin;
        var direction = (rightWallSocket.transform.position - leftWallSocket.transform.position).normalized;
        var origin = leftWallSocket.transform.position + direction * margin/2f;
        
        var picked = new List<Vector3>();
        for (int i = 0; i < amount; i++)
        {
            picked.Add(origin + direction * distance * Random.value);
            Debug.DrawRay(picked.Last(), Vector3.up, Color.green, 1);
        }

        return picked.ToArray();
    }

    private void CleanupObstacles()
    {
        obstacles.ForEach(o => Destroy(o));
        obstacles.Clear();
    }

    private void GenerateObstacles()
    {
        var dir = (leftWallSocket.position - leftWallPivot.position).normalized;
        var dist = Vector3.Distance(leftWallSocket.position, leftWallPivot.position);
        var maxI = Random.Range(5, 10);
        for (var i = 0; i < maxI; i++) {
            var position = leftWallSocket.position + dir * dist * Random.value;
            position += Vector3.right * (5 + Random.value *20);
            position += Vector3.down * Random.value*0.05f;
            var obstacle = Instantiate(forestObstacles[Random.Range(0, forestObstacles.Length-1)]);
            obstacle.transform.position = position;
            obstacle.transform.localScale = Vector3.one * Random.Range(0.6f,1f);
            obstacles.Add(obstacle);
        }

        dir = (rightWallSocket.position - rightWallPivot.position).normalized;
        dist = Vector3.Distance(rightWallSocket.position, rightWallPivot.position);
        maxI = Random.Range(5, 10);
        for (var i = 0; i < maxI; i++) {
            var position = rightWallSocket.position + dir * dist * Random.value;
            position += Vector3.left * (5 + Random.value *20);
            position += Vector3.down * Random.value*0.05f;
            var obstacle = Instantiate(forestObstacles[Random.Range(0, forestObstacles.Length-1)]);
            obstacle.transform.position = position;
            obstacle.transform.localScale = Vector3.one * Random.Range(0.6f,1f);
            obstacles.Add(obstacle);
        }
    }
}
