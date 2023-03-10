using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour {
    public Transform player;
    public Transform point1;
    public Transform point2;
    public Transform point3;
    public int speed;
    public bool atPoint1 = false;
    public bool atPoint2 = false;
    public bool atPoint3 = false;

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

    void Start() {
        transform.position = point1.position;
        atPoint1 = true;
        isCured = false;
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
    
    public void Patrol() {
        float step = speed * Time.deltaTime;

        if (atPoint1) {
            transform.LookAt(point2);
            transform.position = Vector3.MoveTowards(transform.position, point2.position, step); 
        }
        if (transform.position - point2.position == Vector3.zero) {
            atPoint1 = false;
            atPoint2 = true;
        }

        if (atPoint2) {
            transform.LookAt(point3);
            transform.position = Vector3.MoveTowards(transform.position, point3.position, step); 
        }
        if (transform.position - point3.position == Vector3.zero) {
            atPoint2 = false;
            atPoint3 = true;
        }

        if (atPoint3) {
            transform.LookAt(point1);
            transform.position = Vector3.MoveTowards(transform.position, point1.position, step); 
        }
        if (transform.position - point1.position == Vector3.zero) {
            atPoint3 = false;
            atPoint1 = true;
        }
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
        transform.position = Vector3.MoveTowards(transform.position, player.position, step);
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