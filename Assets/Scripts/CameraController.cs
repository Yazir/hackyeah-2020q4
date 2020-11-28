using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    [SerializeField] private Vector3 offset;
    [SerializeField] private Vector3 angleOffset;
    [SerializeField][Range(0, 1)] private float positionLerpStrength;
    [SerializeField][Range(0, 1)] private float angleLerpStrength;

    private Transform primaryTarget;

    private void FixedUpdate()
    {
        if (primaryTarget != null)
        {
            var outPosition = primaryTarget.position;
            outPosition += offset;
            
            var outAngles = angleOffset;

            // transform.position = outPosition;
            // transform.eulerAngles = outAngles;

            transform.position = Vector3.LerpUnclamped(transform.position, outPosition, positionLerpStrength);
            transform.rotation = Quaternion.LerpUnclamped(transform.rotation, Quaternion.Euler(outAngles), angleLerpStrength);
        }
    }

    public void SetTarget(Transform target)
    {
        primaryTarget = target;
    }
}
