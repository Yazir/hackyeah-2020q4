using System.Collections;
using System.Collections.Generic;
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

}
