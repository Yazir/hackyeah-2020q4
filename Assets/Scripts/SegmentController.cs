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

    public Transform CoreSocket => coreSocket;
    public Transform LeftWallSocket => leftWallSocket;
    public Transform RightWallSocket => rightWallSocket;
    public ISegmentParameters LoadedParameters => loadedParameters;

    static private float globalAppliedAngleDeviation = 0;

    private ISegmentParameters loadedParameters;
    private float angle = 1;

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
}
