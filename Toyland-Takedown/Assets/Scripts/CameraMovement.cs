using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMovement : MonoBehaviour
{
    public GameObject target;
    public float rotateSpeed = 5;
    Vector3 offset;
    
    void Start() {
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
        offset = target.transform.position - transform.position;
    }
    
    void LateUpdate() {
        float horizontal = Input.GetAxis("Mouse X") * rotateSpeed;
        target.transform.Rotate(0, horizontal, 0);
        float desiredAngle = target.transform.eulerAngles.y;
        Quaternion rotation = Quaternion.Euler(0, desiredAngle, 0);
        transform.position = target.transform.position - (rotation * offset);
        
        transform.LookAt(target.transform);
    }
}
