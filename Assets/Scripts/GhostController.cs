using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GhostController : MonoBehaviour
{
    public bool FinishedAnimating => finishedAnimating;
    public float OutsideControlFactor => outsideControlFactor;

    private bool finishedAnimating;
    private Transform targetToLateFollow;
    private Vector3 targetToFollowOffset;
    private float outsideControlFactor;

    private void Awake()
    {
        outsideControlFactor = 0;
    }

    public void AnimateCollection(PlayerController player)
    {
        StartCoroutine(GhostCollectCO(player));
    }

    private void LateUpdate()
    {
        if (targetToLateFollow != null)
            transform.position = targetToLateFollow.position + targetToFollowOffset;
    }

    private IEnumerator GhostCollectCO(PlayerController player)
    {
        var offset = transform.position - player.transform.position;
        targetToLateFollow = player.transform;
        targetToFollowOffset = offset;

        StartCoroutine(SlowRotate(Quaternion.LookRotation(offset*-1), 0.5f));

        yield return new WaitForSeconds(0.88f);
        StartCoroutine(TweenOutsideControlFactor(1f, 2f));
        targetToLateFollow = null;
        finishedAnimating = true;
    }

    private IEnumerator SlowRotate(Quaternion target, float step)
    {
        var duration = Quaternion.Angle(target, transform.rotation) / step;
        while (duration > 0)
        {
            transform.rotation = Quaternion.RotateTowards(transform.rotation, target, step);
            duration -= Time.deltaTime;
            yield return new WaitForFixedUpdate();
        }
    }

    private IEnumerator TweenOutsideControlFactor(float target, float duration)
    {
        var step = (target - outsideControlFactor) / duration;
        while (duration > 0)
        {
            var dt = Time.deltaTime;
            outsideControlFactor += step * dt;
            duration -= dt;
            yield return new WaitForEndOfFrame();
        }
    }
}
