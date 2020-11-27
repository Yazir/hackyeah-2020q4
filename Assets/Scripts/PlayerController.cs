using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private Vector3 acceleration;
    [SerializeField] private Vector3 baseMaxSpeeds;

    private Vector3 maxSpeeds;
    private Rigidbody rb;
    private Vector3 velocity;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
    }

    private void FixedUpdate()
    {
        UpdateDifficulty();

        var dt = Time.fixedDeltaTime;

        var tickVelocity = new Vector3();
        var input = GetMovementInput().normalized;
        tickVelocity += Vector3.Scale(input, acceleration);

        tickVelocity += Vector3.forward;
        tickVelocity *= dt;

        velocity += tickVelocity;
        
        // Speed limit
        if (Mathf.Abs(velocity.x) > maxSpeeds.x) velocity.x = maxSpeeds.x * Mathf.Sign(velocity.x);
        if (Mathf.Abs(velocity.z) > maxSpeeds.z) velocity.z = maxSpeeds.z * Mathf.Sign(velocity.z);

        // X axis deceleration
        if (input.x == 0) {
            var factor = acceleration.x * dt;
            if (Mathf.Abs(velocity.x) < factor) velocity.x = 0;
            else velocity.x -= Mathf.Sign(velocity.x) * factor;
        }

        rb.velocity = velocity;
    }

    private Vector3 GetMovementInput() {
        var input = new Vector3();
        // if (Input.GetKey(KeyCode.W)) input.z += 1;
        // if (Input.GetKey(KeyCode.S)) input.z -= 1;
        if (Input.GetKey(KeyCode.A)) input.x -= 1;
        if (Input.GetKey(KeyCode.D)) input.x += 1;

        return input;
    }

    private void UpdateDifficulty() {
        maxSpeeds = baseMaxSpeeds + new Vector3(0, 0, transform.position.z / 10);
    }
}
