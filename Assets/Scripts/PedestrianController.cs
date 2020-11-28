using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PedestrianController : MonoBehaviour
{
    [SerializeField] private float speed = 2;

    private Rigidbody rb;
    private Vector3 moveDirection;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
        moveDirection = (-Vector3.forward + new Vector3(Random.value-0.5f, 0, Random.value-0.5f) * 2f).normalized;
    }

    private void FixedUpdate()
    {
        rb.velocity = moveDirection * speed;
    }
}
