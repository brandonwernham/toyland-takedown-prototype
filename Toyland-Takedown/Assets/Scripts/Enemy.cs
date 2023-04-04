using System.Collections;
using System.Collections.Generic;
using TMPro;
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

    public bool isDead;

    public float timeToDestroy = 2f;

    public float patrolRadius = 5f;
    public float patrolSpeed = 1f;

    private float patrolAngle;
    private Vector3 center;
    private Vector3 previousPatrolPosition;
    public bool returningToPatrol;
    public AudioSource audioSource;
    public AudioClip dieSound;
    int timesToPlay = 1;

    void Start() {
        isDead = false;

        player = GameObject.FindWithTag("Player").transform;
        playerRef = GameObject.FindWithTag("Player");

        center = transform.position;
        patrolAngle = Random.Range(0f, 360f);
    }

    void Update() {
        if (!isDead) {
            FieldOfViewCheck();
        }
        
        if (spottedPlayer && !isDead) {
            ChasePlayer();
        } else if (isDead) {
            Die();
        } else if (returningToPatrol && !spottedPlayer) {
            ReturnToPatrol();
        } else if (!spottedPlayer && !returningToPatrol) {
            Patrol();
        }
    }

    void OnTriggerEnter(Collider other) {
        if (other.CompareTag("Cure")) {
            Destroy(other.gameObject);
            isDead = true;
        }
    }

    void OnCollisionEnter(Collision other) {
        if (other.gameObject.CompareTag("Player")) {
            print("player");
        }
    }
    
    public void Patrol() {
        previousPatrolPosition = transform.position;

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

    public void ReturnToPatrol() {
        float step = patrolSpeed * Time.deltaTime * 0.2f;

        transform.position = Vector3.MoveTowards(transform.position, previousPatrolPosition, step);
        transform.LookAt(previousPatrolPosition);

        if (Vector3.Distance(transform.position, previousPatrolPosition) < 0.1f)
        {
            returningToPatrol = false;
        }
    }

    public void FieldOfViewCheck() {
        Collider[] rangeChecks = Physics.OverlapSphere(transform.position, radius, targetMask);

        if (rangeChecks.Length != 0) { // If the player is within the radius
            Transform target = rangeChecks[0].transform;
            Vector3 directionToTarget = (target.position - transform.position).normalized;

            if (Vector3.Angle(transform.forward, directionToTarget) < angle / 2) { // If the player is within the radius and within the angle
                float distanceToTarget = Vector3.Distance(transform.position, target.position);

                if (!Physics.Raycast(transform.position, directionToTarget, distanceToTarget, obstructionMask)) { // If there is no obstruction
                    spottedPlayer = true;
                    playerRef.GetComponent<PlayerMovement>().isSpotted = true;
                } else { // If there is an obstruction
                    spottedPlayer = false;
                    playerRef.GetComponent<PlayerMovement>().isSpotted = false;
                    returningToPatrol = true;
                }
            } else { // If the player is within the raduis but NOT within the angle
                spottedPlayer = false;
                playerRef.GetComponent<PlayerMovement>().isSpotted = false;
            }
        } else if (spottedPlayer) { // If the player is completely out of range and still spotted, return to patrol
            spottedPlayer = false;
            playerRef.GetComponent<PlayerMovement>().isSpotted = false;
            returningToPatrol = true;
        }
    }

    public void ChasePlayer() {
        float step = chasingSpeed * Time.deltaTime;

        transform.LookAt(player);
        transform.position = Vector3.MoveTowards(transform.position, new Vector3(player.position.x, transform.position.y, player.position.z), step);
    }

    public void Die() {
        timeToDestroy -= Time.deltaTime;
        
        if (timesToPlay > 0) {
            audioSource.PlayOneShot(dieSound);
            timesToPlay--;
        }
        
        Animator anim = gameObject.GetComponent<Animator>();
        anim.enabled = false;
        if (timeToDestroy <= 0) {
            spottedPlayer = false;
            playerRef.GetComponent<PlayerMovement>().isSpotted = false;
            EnemyCount.enemyCount -= 1;
            print("Enemies Left: " + EnemyCount.enemyCount);

            if (EnemyCount.enemyCount > 0) {
                GemCount.gemCount += 1;
            }
            
            Destroy(gameObject);
        }
    }
}