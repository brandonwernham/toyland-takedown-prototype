using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour {
    public Transform player;
    public Transform point1;
    public Transform point2;
    public Transform point3;
    public Collider spotted;
    public int speed;
    public bool atPoint1 = false;
    public bool atPoint2 = false;
    public bool atPoint3 = false;

    void Start() {
        transform.position = point1.position;
        atPoint1 = true;
    }

    void Update() {
        Patrol();

        if (spotted.CompareTag("Player")) {
            Chase();
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

    public void Chase() {

    }
}
