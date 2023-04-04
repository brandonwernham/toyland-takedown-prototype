using System.Collections;
using System.Collections.Generic;
using TMPro;
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

    public GameObject gem;

    public bool isSpotted;
    public bool isRunning;
    public bool isJumping;
    public TMP_Text cannotPlaceText;
    float timeToDeleteText = 1;
    public bool countDown = false;

    void Start() {
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
        rb = GetComponent<Rigidbody>();
        capCollider = transform.GetComponent<CapsuleCollider>();
        readyToJump = true;
        currentHealth = maxHealth;
        healthBar.SetMaxHealth(maxHealth);
        isSpotted = false;

        GemCount.gemCount = 4;
        if (UpgradeManager.gemUpgrade1) {
            GemCount.gemCount = 20;
        }
        if (UpgradeManager.gemUpgrade2) {
            GemCount.gemCount = 100;
        }

        timeToDeleteText = 4;
        cannotPlaceText.text = "Cure bots by pressing 'E' to drop a gem!";
        countDown = true;
    }

    void Update() {
        MyInput();
        DragControl();
        SpeedControl();
        PlaceGem();

        if (countDown) {
            CountDown();
        }
    }

    void FixedUpdate() {
        PlayerPhysics();
        Jump();
    }

    void OnCollisionEnter(Collision other) {
        if (other.gameObject.CompareTag("Enemy") && !other.gameObject.GetComponent<Enemy>().isDead) {
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

        if (!(moveX != 0 || moveZ != 0 && isGrounded) && !isJumping) {
            anim.SetBool("isIdle", true);
            anim.SetBool("isWalking", false);
            anim.SetBool("isRunning", false);
            isRunning = false;
        }
        else if ((moveX != 0 || moveZ != 0 && isGrounded) && !isJumping && !isRunning) {
            anim.SetBool("isIdle", false);
            anim.SetBool("isWalking", true);
            anim.SetBool("isRunning", false);
        }

        if (isRunning && !isJumping) {
            anim.SetBool("isIdle", false);
            anim.SetBool("isWalking", false);
            anim.SetBool("isRunning", true);
        }

        if (Input.GetKeyDown(KeyCode.W)) {
            if (!isRunning && !isJumping) {
                anim.SetBool("isIdle", false);
                anim.SetBool("isWalking", true);
                anim.SetBool("isRunning", false);
            }
            Run();
        }
        else if (Input.GetKeyDown(KeyCode.S) && !isJumping) {
            isRunning = false;
            moveSpeed = 5f;
            anim.SetBool("isIdle", false);
            anim.SetBool("isWalking", true);
            anim.SetBool("isRunning", false);
        }
        else if (!Input.GetKey(KeyCode.W) && Input.GetKeyDown(KeyCode.A) && !isJumping) {
            isRunning = false;
            moveSpeed = 5f;
            anim.SetBool("isIdle", false);
            anim.SetBool("isWalking", true);
            anim.SetBool("isRunning", false);
        }
        else if (!Input.GetKey(KeyCode.W) && Input.GetKeyDown(KeyCode.D) && !isJumping) {
            isRunning = false;
            moveSpeed = 5f;
            anim.SetBool("isIdle", false);
            anim.SetBool("isWalking", true);
            anim.SetBool("isRunning", false);
        }
    }

    public void Run() {
        float timeSinceTap = Time.time - previousTap;

        if (timeSinceTap <= doubleTapTime) {
            isRunning = true;
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
        if (!isGrounded) {
            isJumping = true;
            anim.SetBool("isIdle", false);
            anim.SetBool("isWalking", false);
            anim.SetBool("isRunning", false);
            anim.SetBool("isJumping", true);
        }
        else {
            isJumping = false;
            anim.SetBool("isJumping", false);
        }

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
        CoinCount.coinCount -= CoinCount.coinsThisGame;
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void PlaceGem() {
        Vector3 playerPos = new Vector3(transform.position.x, transform.position.y + 1f, transform.position.z);
        if (Input.GetKeyDown(KeyCode.E) && !isSpotted && GemCount.gemCount > 0) {
            Instantiate(gem, playerPos, Quaternion.Euler(0, 0, 0));
            GemCount.gemCount -= 1;
            print("Gems left: " + GemCount.gemCount);
        } else if (Input.GetKeyDown(KeyCode.E) && isSpotted && GemCount.gemCount > 0) {
            cannotPlaceText.text = "Can't place gem while being chased!";
            countDown = true;
        } else if (Input.GetKeyDown(KeyCode.E) && GemCount.gemCount <= 0) {
            cannotPlaceText.text = "Out of gems! Better restart...";
            countDown = true;
        }
    }

    public void CountDown() {
        timeToDeleteText -= Time.deltaTime;
        if (timeToDeleteText <= 0) {
            cannotPlaceText.text = "";
            timeToDeleteText = 1;
            countDown = false;
        }
    }
}