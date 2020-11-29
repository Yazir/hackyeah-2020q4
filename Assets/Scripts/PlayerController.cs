using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private Vector3 acceleration;
    [SerializeField] private Vector3 baseMaxSpeeds;
    [SerializeField] private GhostCatcherWrapper ghostCatcher;
    [SerializeField] private GameObject ghostPrefab;
    [SerializeField] private Transform visualObject;

    public float ZDistanceTravelledLastTick => zDistanceTravelledLastTick;

    private Vector3 maxSpeeds;
    private Rigidbody rb;
    private Vector3 velocity;
    private float zDistanceTravelledLastTick;
    private Vector3 lastPosition;
    private List<GhostController> collectedGhosts;

    private void Awake()
    {
        GameContext.instance.CameraController.SetTarget(transform);

        rb = GetComponent<Rigidbody>();
        collectedGhosts = new List<GhostController>();
        lastPosition = transform.position;
        ghostCatcher.onTriggerEnter += OnGhostCatcherTriggerEnter;
    }

    private void FixedUpdate()
    {
        UpdateDifficulty();
        UpdateMovement();
        UpdateGhosts();
        UpdateVisualObject();
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

        if (finishTarget != null) {
            velocity = (finishTarget.transform.position - transform.position).normalized * 0.5f;
        }
        else {
            rb.velocity = Vector3.zero;
        } 
        
        rb.MovePosition(rb.position + velocity * dt);
        
        // zDistanceTravelledLastTick = velocity.magnitude * dt;
        zDistanceTravelledLastTick = Mathf.Max(0, transform.position.z - lastPosition.z);
        lastPosition = transform.position;
    }

    private void UpdateVisualObject() {
        visualObject.forward = velocity.normalized;
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
        maxSpeeds = baseMaxSpeeds + new Vector3(0, 0, transform.position.z / 100);
    }

    private void OnGhostCatcherTriggerEnter(Collider pedCollider)
    {
        var ped = pedCollider.attachedRigidbody.GetComponent<PedestrianController>();
        var ghost = SpawnAndCollectGhost(ped.VisualObject);

        var camera = GameContext.instance.CameraController;
        camera.SetFocusTarget(ghost.transform, 0.8f);
        camera.runtimeOffset = (camera.Offset.normalized + Vector3.back * 0.75f) * collectedGhosts.Count / 6f;
        
        ghost.transform.position = ped.transform.position;
    }

    private GhostController SpawnAndCollectGhost(Transform at) {
        var ghost = Instantiate(ghostPrefab).GetComponent<GhostController>();
        ghost.transform.position = at.position;
        ghost.transform.rotation = at.rotation;

        if (collectedGhosts.Count % 2 == 0) collectedGhosts.Add(ghost);
        else collectedGhosts.Insert(0, ghost);

        ghost.AnimateCollection(this);
        return ghost;
    }

    private void UpdateGhosts() {
        var spacing = 0.13f;

        var animable = collectedGhosts.Where(g => g.FinishedAnimating).ToArray();
        var maxIndex = animable.Length;
        for (int i = 0; i < maxIndex; i++)
        {
            var ghost = animable[i];
            if (!ghost.FinishedAnimating)
                continue;

            var origin = transform.position + Vector3.back*0.35f + rb.velocity * Time.fixedDeltaTime;
            var outPosition = origin + Vector3.right * i*spacing;
            outPosition += Vector3.left * maxIndex / 2f * spacing;
            outPosition += Vector3.down * 0.15f;
            outPosition += new Vector3(Random.value - 0.5f, (Random.value - 0.5f) * 0.2f, Random.value - 0.5f) * 0.015f;
          
            var sideFactor = maxIndex > 1 ? Mathf.Abs(i - (maxIndex-1)/2f) / ((maxIndex-1)/2f) : 1;
            outPosition += Vector3.back * maxIndex*0.15f * sideFactor;
            
            
            var ghostToPlayer = transform.position - ghost.transform.position;
            ghostToPlayer.y = 0;
            var outRotation = Quaternion.LookRotation(ghostToPlayer);

            var controlFactor = ghost.OutsideControlFactor;
            ghost.transform.position = Vector3.Lerp(ghost.transform.position, outPosition, (0.6f - sideFactor*0.35f) * controlFactor);
            ghost.transform.rotation = Quaternion.Lerp(ghost.transform.rotation, outRotation, 0.1f * controlFactor);
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.GetComponent<Collider>().tag == "Finito") {
            if (startedFinishSequence == false) {
                startedFinishSequence = true;
                finishTarget = other.transform;
                GameContext.instance.FadeOut();
                StartCoroutine(FinishSequenceCO());
            }
        }
    }

    private Transform finishTarget;
    private bool startedFinishSequence = false;
    private IEnumerator FinishSequenceCO() {
        yield return new WaitForSeconds(25);
        Application.Quit();
    }
}
