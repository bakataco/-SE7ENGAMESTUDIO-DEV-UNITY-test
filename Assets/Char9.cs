using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    // Animation
    public Animator animator;

    // Camera
    public float mouseSensitivity = 2f;
    public FreeLookMouseControl cameraControl;

    // Ground Movement
    private Rigidbody rb;
    public float MoveSpeed = 5f;
    private float moveHorizontal;
    private float moveForward;

    // Jumping
    public float jumpForce = 10f;
    public float fallMultiplier = 2.5f;
    public float ascendMultiplier = 2f;
    private bool isGrounded = true;
    public LayerMask groundLayer;
    private float groundCheckTimer = 0f;
    private float groundCheckDelay = 0.3f;
    private float playerHeight;
    private float raycastDistance;

    //Interaction
    public Camera playerCamera;
    public GameObject interactionIcon;
    private IInteractable currentInteractable;

    //State
    private bool isDead = false;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        rb.freezeRotation = true;
        animator = GetComponent<Animator>();
        interactionIcon.SetActive(false);

        // Hides the mouse
        //Cursor.lockState = CursorLockMode.Locked;
        //Cursor.visible = false;

        // Set the raycast to be slightly beneath the player's feet
        playerHeight = GetComponent<CapsuleCollider>().height
            * transform.localScale.y;

        raycastDistance = (playerHeight / 2) + 0.2f;
    }

    void Update()
    {
        if (isDead) return;

        if (!isDead)
        {
            moveHorizontal = Input.GetAxisRaw("Horizontal");
            moveForward = Input.GetAxisRaw("Vertical");

            // Rotate character with mouse
            if (!Input.GetMouseButton(0))
            {
                float horizontalRotation = Input.GetAxis("Mouse X") * mouseSensitivity;
                transform.Rotate(0f, horizontalRotation, 0f);
            }
        }

        // Jump
        if (Input.GetButtonDown("Jump") && isGrounded)
        {
            Jump();
        }

        // Ground checking
        if (!isGrounded && groundCheckTimer <= 0f)
        {
            Vector3 rayOrigin = transform.position + Vector3.up * 0.1f;

            isGrounded = Physics.Raycast(rayOrigin, Vector3.down, raycastDistance, groundLayer);
        }
        else
        {
            groundCheckTimer -= Time.deltaTime;
        }

        if (isGrounded)
        {
            animator.SetBool("is jumping", false);
        }
        else
        {
            animator.SetBool("is jumping", true);
        }
    }

    public void SetInteractable(IInteractable interactable)
    {
        currentInteractable = interactable;
        interactionIcon.SetActive(currentInteractable != null);
    }

    public void ClearInteractable(IInteractable interactable)
    {
        if (currentInteractable == interactable)
        {
            currentInteractable = null;
            interactionIcon.SetActive(false);
        }
    }

    void FixedUpdate()
    {
        MovePlayer();
        ApplyJumpPhysics();
    }

    void MovePlayer()
    {
        Vector3 movement = (transform.right * moveHorizontal + transform.forward * moveForward).normalized;
        Vector3 targetVelocity = movement * MoveSpeed;

        // Apply movement to the Rigidbody
        Vector3 velocity = rb.velocity;

        velocity.x = targetVelocity.x;
        velocity.z = targetVelocity.z;

        rb.velocity = velocity;

        if (moveHorizontal != 0 || moveForward != 0)
        {
            animator.SetBool("is running", true);
        }

        // Stop sliding when grounded and not moving
        if (isGrounded && moveHorizontal == 0 && moveForward == 0)
        {
            rb.velocity = new Vector3(0, rb.velocity.y, 0);
            animator.SetBool("is running", false);
        }
    }

    void Jump()
    {
        isGrounded = false;
        groundCheckTimer = groundCheckDelay;

        rb.velocity = new Vector3(rb.velocity.x, jumpForce, rb.velocity.z);
    }

    void ApplyJumpPhysics()
    {
        if (rb.velocity.y < 0)
        {
            // Falling
            rb.velocity += Vector3.up * Physics.gravity.y * fallMultiplier * Time.fixedDeltaTime;
        }
        else if (rb.velocity.y > 0)
        {
            // Rising
            rb.velocity += Vector3.up * Physics.gravity.y * ascendMultiplier * Time.fixedDeltaTime;
        }
    }

    public void InteractWithCurrentObject()
    {
        if (currentInteractable != null)
        {
            currentInteractable.Interact();
        }
    }

    public void Die()
    {
        if (isDead) return;

        isDead = true;

        // Stop player movement
        rb.velocity = Vector3.zero;
        rb.isKinematic = true;

        // Disable mouse camera control
        cameraControl.DisableCameraControl();

        // Play death animation
        animator.SetTrigger("is dead");

    }
}


