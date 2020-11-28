using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PedestrianController : MonoBehaviour
{
    [SerializeField] private float speed = 2;
    [SerializeField] private Transform visualObject;

    public Transform VisualObject => visualObject;

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

        var lookDirection = rb.velocity;
        lookDirection.y = 0;
        lookDirection = lookDirection.normalized;
        visualObject.forward = lookDirection;
    }

    private void OnCollisionEnter(Collision other)
    {
        if (other.collider.gameObject.layer == LayerMask.NameToLayer("SideWall")) {
            var segment = other.transform.parent;
            var dirToCenter = (segment.position - Vector3.forward * 10 - transform.position).normalized;
            moveDirection += dirToCenter * 2f;
        }
    }
}
