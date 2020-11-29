using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    [SerializeField] private Vector3 offset;
    [SerializeField] private Vector3 angleOffset;
    [SerializeField][Range(0, 1)] private float positionLerpStrength;
    [SerializeField][Range(0, 1)] private float angleLerpStrength;

    public Vector3 Offset => offset;
    public Vector3 runtimeOffset;
    
    private Transform primaryTarget;
    private Transform focusTarget;
    private IEnumerator focusCO;

    private void FixedUpdate()
    {
        if (primaryTarget != null)
        {
            var outPosition = primaryTarget.position;
            outPosition += offset + runtimeOffset;
            
            var outAngles = angleOffset;

            transform.position = Vector3.LerpUnclamped(transform.position, outPosition, positionLerpStrength);

            if (focusTarget != null)
            {
                var outRotation = Quaternion.LookRotation(focusTarget.position - transform.position);
                transform.rotation = Quaternion.LerpUnclamped(transform.rotation, outRotation, angleLerpStrength * 0.05f);
            }
            else
            {
                transform.rotation = Quaternion.LerpUnclamped(transform.rotation, Quaternion.Euler(outAngles), angleLerpStrength);
            }
        }
    }

    public void SetTarget(Transform target)
    {
        primaryTarget = target;
    }
    
    public void SetFocusTarget(Transform target, float duration)
    {
        focusTarget = target;

        if (focusCO != null) StopCoroutine(focusCO);
        focusCO = FocusCO(target, duration);
        StartCoroutine(focusCO);
    }

    private IEnumerator FocusCO(Transform target, float duration) {
        yield return new WaitForSeconds(duration);
        focusTarget = null;
        focusCO = null;
    }
}
