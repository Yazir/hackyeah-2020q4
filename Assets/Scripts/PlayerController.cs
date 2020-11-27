using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    private Rigidbody rb;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
    }


    private void FixedUpdate()
    {
        var dt = Time.fixedDeltaTime;

        var outMovement = Vector3.forward;
        
        var input = GetMovementInput();
        outMovement.x += input.x;
        
        rb.MovePosition(rb.position + outMovement * dt);
    }

    private Vector2 GetMovementInput() {
        var input = new Vector2();
        if (Input.GetKey(KeyCode.W)) input.y -= 1;
        if (Input.GetKey(KeyCode.S)) input.y += 1;
        if (Input.GetKey(KeyCode.A)) input.x -= 1;
        if (Input.GetKey(KeyCode.D)) input.x += 1;

        return input;
    }
}
