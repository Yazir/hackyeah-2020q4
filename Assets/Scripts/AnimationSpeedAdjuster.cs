using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimationSpeedAdjuster : MonoBehaviour
{
    [SerializeField] private Animator animator;
    [SerializeField] private float baseSpeed = 1;
    [SerializeField] private float modifier = 100;

    private Vector3 lastPosition;
    private float speed;

    private void Awake()
    {
        lastPosition = transform.position;
    }

    private void FixedUpdate()
    {

        if (modifier == 0) modifier = 1;

        var currentPosition = transform.position;
        speed = Vector3.Distance(lastPosition, currentPosition) / Time.fixedDeltaTime;

        animator.SetFloat("speed",speed);
        animator.speed = baseSpeed + speed / modifier;
        lastPosition = currentPosition;
    }
}
