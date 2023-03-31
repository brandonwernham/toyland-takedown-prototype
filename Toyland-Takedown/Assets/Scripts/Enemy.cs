using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour {
    public Transform player;

    public float radius;
    [Range(0, 360)]
    public float angle;
    public GameObject playerRef;
    public LayerMask targetMask;
    public LayerMask obstructionMask;
    public bool spottedPlayer;
    public int chasingSpeed;

    public bool isCured;

    public float timeToDestroy = 5f;

    public float patrolRadius = 5f;
    public float patrolSpeed = 1f;

    private float patrolAngle;
    private Vector3 center;

    void Start() {
        isCured = false;

        player = GameObject.FindWithTag("Player").transform;
        playerRef = GameObject.FindWithTag("Player");

        center = transform.position;
        patrolAngle = Random.Range(0f, 360f);
    }

    void Update() {
        if (!isCured) {
            FieldOfViewCheck();
        }
        
        if (spottedPlayer && !isCured) {
            ChasePlayer();
        } else if (isCured) {
            JumpUpAndDown();
        } else {
            Patrol();
        }
    }

    void OnTriggerEnter(Collider other) {
        if (other.CompareTag("Cure")) {
            Destroy(other.gameObject);
            isCured = true;
        }
    }

    void OnCollisionEnter(Collision other) {
        if (other.gameObject.CompareTag("Player")) {
            print("player");
        }
    }
    
    public void Patrol() {
        patrolAngle += patrolSpeed * Time.deltaTime;
        if (patrolAngle >= 360f)
        {
            patrolAngle -= 360f;
        }

        float x = center.x + patrolRadius * Mathf.Cos(patrolAngle * Mathf.Deg2Rad);
        float z = center.z + patrolRadius * Mathf.Sin(patrolAngle * Mathf.Deg2Rad);

        Vector3 targetPosition = new Vector3(x, transform.position.y, z);
        transform.position = targetPosition;

        Vector3 direction = new Vector3(Mathf.Cos((patrolAngle + 90) * Mathf.Deg2Rad), 0, Mathf.Sin((patrolAngle + 90) * Mathf.Deg2Rad));

        transform.rotation = Quaternion.LookRotation(direction);
    }

    public void FieldOfViewCheck() {
        Collider[] rangeChecks = Physics.OverlapSphere(transform.position, radius, targetMask);

        if (rangeChecks.Length != 0) {
            Transform target = rangeChecks[0].transform;
            Vector3 directionToTarget = (target.position - transform.position).normalized;

            if (Vector3.Angle(transform.forward, directionToTarget) < angle / 2) {
                float distanceToTarget = Vector3.Distance(transform.position, target.position);

                if (!Physics.Raycast(transform.position, directionToTarget, distanceToTarget, obstructionMask)) {
                    spottedPlayer = true;
                    playerRef.GetComponent<PlayerMovement>().isSpotted = true;
                } else {
                    spottedPlayer = false;
                    playerRef.GetComponent<PlayerMovement>().isSpotted = false;
                }
            } else {
                spottedPlayer = false;
                playerRef.GetComponent<PlayerMovement>().isSpotted = false;
            }
        } else if (spottedPlayer) {
            spottedPlayer = false;
            playerRef.GetComponent<PlayerMovement>().isSpotted = false;
        }
    }

    public void ChasePlayer() {
        float step = chasingSpeed * Time.deltaTime;

        transform.LookAt(player);
        transform.position = Vector3.MoveTowards(transform.position, new Vector3(player.position.x, transform.position.y, player.position.z), step);
    }

    public void JumpUpAndDown() {
        float y = Mathf.PingPong(Time.time, 0.5f) * 6 - 3;
        transform.position = new Vector3(transform.position.x, y + 3f, transform.position.z);
        timeToDestroy -= Time.deltaTime;
        if (timeToDestroy <= 0) {
            spottedPlayer = false;
            playerRef.GetComponent<PlayerMovement>().isSpotted = false;
            Destroy(gameObject);
        }
    }
}