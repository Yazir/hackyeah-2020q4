using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private Vector3 acceleration;
    [SerializeField] private Vector3 baseMaxSpeeds;
    [SerializeField] private GhostCatcherWrapper ghostCatcher;
    [SerializeField] private GameObject ghostPrefab;

    public float ZDistanceTravelledLastTick => zDistanceTravelledLastTick;

    private Vector3 maxSpeeds;
    private Rigidbody rb;
    private Vector3 velocity;
    private float zDistanceTravelledLastTick;
    private Vector3 lastPosition;
    private List<Transform> collectedGhosts;

    private void Awake()
    {
        GameContext.instance.CameraController.SetTarget(transform);

        rb = GetComponent<Rigidbody>();
        collectedGhosts = new List<Transform>();
        lastPosition = transform.position;
        ghostCatcher.onTriggerEnter += OnGhostCatcherTriggerEnter;



        SpawnGhost();
        SpawnGhost();
        SpawnGhost();
        // SpawnGhost();
        // SpawnGhost();
        // SpawnGhost();
        // SpawnGhost();
        // SpawnGhost();
        // SpawnGhost();
        // SpawnGhost();
        // SpawnGhost();
        // SpawnGhost();
        // SpawnGhost();
        // SpawnGhost();
        // SpawnGhost();
        // SpawnGhost();
        // SpawnGhost();
    }

    private void FixedUpdate()
    {
        UpdateDifficulty();
        UpdateMovement();
        UpdateGhosts();
    }

    private void UpdateMovement() {
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
        
        // zDistanceTravelledLastTick = velocity.magnitude * dt;
        zDistanceTravelledLastTick = Mathf.Max(0, transform.position.z - lastPosition.z);
        lastPosition = transform.position;
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

    private void OnGhostCatcherTriggerEnter(Collider pedCollider)
    {
        var ped = pedCollider.attachedRigidbody.GetComponent<PedestrianController>();
        var ghost = SpawnGhost();
        ghost.transform.position = ped.transform.position;
        print("Ped caught");
    }

    private GameObject SpawnGhost() {
        var ghost = Instantiate(ghostPrefab);
        collectedGhosts.Add(ghost.transform);
        return ghost;
    }

    private void UpdateGhosts() {
        var spacing = 0.13f;
        var maxIndex = collectedGhosts.Count;
        for (int i = 0; i < maxIndex; i++)
        {
            var ghost = collectedGhosts[i];

            var origin = transform.position + Vector3.back*0.25f + rb.velocity * Time.fixedDeltaTime;
            var outPosition = origin + Vector3.right * i*spacing;
            outPosition += Vector3.left * maxIndex / 2f * spacing;
            outPosition += Vector3.down * 0.15f;
            outPosition += new Vector3(Random.value - 0.5f, (Random.value - 0.5f) * 0.2f, Random.value - 0.5f) * 0.015f;
          
            var sideFactor = Mathf.Abs(i - (maxIndex-1)/2f) / ((maxIndex-1)/2f);
            outPosition += Vector3.back * maxIndex*0.15f * sideFactor;

            ghost.position = Vector3.Lerp(ghost.position, outPosition, 0.6f - sideFactor*0.35f);
        }
    }
}
