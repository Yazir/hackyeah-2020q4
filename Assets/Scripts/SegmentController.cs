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
    [SerializeField] private Transform rightWall;
    [SerializeField] private Transform rightWallPivot;
    [SerializeField] private Transform rightWallSocket;

    public Transform CoreSocket => coreSocket;
    public Transform LeftWallSocket => leftWallSocket;
    public Transform RightWallSocket => rightWallSocket;
    
    private float angle = 1;

    public void AlignToSegment(SegmentController otherSegment) {
        var corePivotOffset = corePivot.transform.position - transform.position;
        transform.position = otherSegment.CoreSocket.transform.position - corePivotOffset;

        leftWall.localEulerAngles = new Vector3(0, angle, 0);
        var leftWallPivotOffset = leftWallPivot.transform.position - leftWall.transform.position;
        leftWall.transform.position = otherSegment.leftWallSocket.transform.position - leftWallPivotOffset;

        rightWall.localEulerAngles = new Vector3(0, -angle, 0);
        var rightWallPivotOffset = rightWallPivot.transform.position - rightWall.transform.position;
        rightWall.transform.position = otherSegment.rightWallSocket.transform.position - rightWallPivotOffset;
    }

    public Vector3[] GetSpawnpoints(int amount = 1) {
        // var available = spawnpoints.ToList();
        // var picked = new List<Vector3>();

        // for (int i = 0; i < amount; i++)
        // {
        //     var index = Random.Range(0, available.Count - 1);
        //     picked.Add(available[index].position);
        //     available.RemoveAt(index);
        // }
        
        // return picked.ToArray();


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
