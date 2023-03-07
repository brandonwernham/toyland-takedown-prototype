using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerMovement : MonoBehaviour {
    public float moveSpeed = 5f;
    Rigidbody rb;
    float moveX;
    float moveZ;

    private float previousTap;
    public float doubleTapTime = 0.2f;

    public int jumpForce;
    public float jumpCooldown;
    public float airMultiplier;
    bool readyToJump;
    private CapsuleCollider capCollider;
    public LayerMask groundLayerMask;
    public bool isGrounded;

    public Transform camOrientation;
    public Transform playerOrientation;
    Vector3 moveDir;
    public float drag;

    public float knockbackForce;

    public Transform cam;
    
    public Animator anim;

    public int maxHealth;
    public int currentHealth;
    public HealthBar healthBar;

    public GameObject cure;

    public bool isSpotted;

    void Start() {
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
        rb = GetComponent<Rigidbody>();
        capCollider = transform.GetComponent<CapsuleCollider>();
        readyToJump = true;
        currentHealth = maxHealth;
        healthBar.SetMaxHealth(maxHealth);
        isSpotted = false;
    }

    void Update() {
        MyInput();
        DragControl();
        SpeedControl();
        PlaceCure();
    }

    void FixedUpdate() {
        PlayerPhysics();
        Jump();
    }

    void OnCollisionEnter(Collision other) {
        if (other.gameObject.CompareTag("Enemy") && !other.gameObject.GetComponent<Enemy>().isCured) {
            TakeDamage(1);
            Knockback();
        }
    }

    private void PlayerPhysics() {
        
        isGrounded = Physics.Raycast(capCollider.bounds.center, Vector3.down, 1.1f, groundLayerMask);
        moveDir = playerOrientation.forward * moveZ + playerOrientation.right * moveX;
        
        if (isGrounded) {
            rb.AddForce(moveDir.normalized * moveSpeed * 10f, ForceMode.Force);
        }
        else if (!isGrounded) {
            rb.AddForce(moveDir.normalized * moveSpeed * 10f * airMultiplier, ForceMode.Force);
        }
    }

    private void MyInput() {
        moveX = Input.GetAxisRaw("Horizontal");
        moveZ = Input.GetAxisRaw("Vertical");

        Vector3 camDirection = new Vector3(camOrientation.eulerAngles.x, camOrientation.eulerAngles.y, camOrientation.eulerAngles.z);
        float targetAngle = Mathf.Atan2(camDirection.x, camDirection.z) * Mathf.Rad2Deg + cam.eulerAngles.y;
        transform.rotation = Quaternion.Euler(0f, targetAngle - 90f, 0f);

        if (moveX != 0 || moveZ != 0 && isGrounded) {
            anim.SetFloat("speed", 1);
        }
        else {
            anim.SetFloat("speed", 0);
        }

        if (Input.GetKeyDown(KeyCode.W)) {
            Run();
        }
        else if (Input.GetKeyDown(KeyCode.S)) {
            moveSpeed = 5f;
        }
        else if (!Input.GetKey(KeyCode.W) && Input.GetKeyDown(KeyCode.A)) {
            moveSpeed = 5f;
        }
        else if (!Input.GetKey(KeyCode.W) && Input.GetKeyDown(KeyCode.D)) {
            moveSpeed = 5f;
        }
    }

    public void Run() {
        float timeSinceTap = Time.time - previousTap;

        if (timeSinceTap <= doubleTapTime) {
            moveSpeed = 8f;
        }
        else {
            moveSpeed = 5f;
        }

        previousTap = Time.time;
    }

    private void DragControl() {
        if (isGrounded) {
            rb.drag = drag;
        }
        else {
            rb.drag = 0;
        }
    }

    private void SpeedControl() {
        Vector3 constVelocity = new Vector3(rb.velocity.x, 0f, rb.velocity.z);

        if (constVelocity.magnitude > moveSpeed) {
            Vector3 limitSpeed = constVelocity.normalized * moveSpeed;
            rb.velocity = new Vector3(limitSpeed.x, rb.velocity.y, limitSpeed.z);
        }
    }

    private void Jump() {
        if (Input.GetKey(KeyCode.Space) && readyToJump && isGrounded) {
            
            rb.velocity = new Vector3(rb.velocity.x, 0f, rb.velocity.z);
            rb.AddForce(transform.up * jumpForce, ForceMode.Impulse);
            readyToJump = false;
            Invoke(nameof(ResetJump), jumpCooldown);
        }
    }

    private void ResetJump() {
        readyToJump = true;
    }

    public void Knockback() {
        rb.AddForce(transform.up * knockbackForce, ForceMode.Impulse);
    }

    public void TakeDamage(int damage) {
        currentHealth -= damage;
        healthBar.SetHealth(currentHealth);

        if (currentHealth <= 0) {
            Die();
        }
    }

    public void Die() {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void PlaceCure() {
        Vector3 playerPos = new Vector3(transform.position.x, transform.position.y + 1f, transform.position.z);
        if (Input.GetKeyDown(KeyCode.E) && !isSpotted) {
            Instantiate(cure, playerPos, Quaternion.Euler(0, 0, 0));
        }
    }
}