using System;
using UnityEngine;
using UnityEngine.SceneManagement;  // Add this line

public class PlayerMovement : MonoBehaviour
{
    public float moveSpeed = 5f;
    public float jumpForce = 10f;
    public Transform groundCheck;
    public LayerMask groundLayer;
    public DialogueObject jumpDialogue;

    public DialogueExposition dialogueExposition;

    private bool dialougeRan = false;

    private float lastClickTime = 0f;
    [SerializeField]  private float doubleClickThreshold = 0.3f;

    private Rigidbody2D rb;
    private bool isGrounded;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        Move();
        Jump();
    }

    void Move()
    {
        float moveInput = Input.GetAxis("Horizontal");
        rb.linearVelocity = new Vector2(moveInput * moveSpeed, rb.linearVelocity.y);
    }

    void Jump()
    {
        // Check if we are in "Level_1"
        if (SceneManager.GetActiveScene().name != "Level_1")
        {
            isGrounded = Physics2D.OverlapCircle(groundCheck.position, 0.2f, groundLayer);
        }

        // Allow jumping infinitely in "Level_1" without checking grounded status
        if (Input.GetButtonDown("Jump") && (SceneManager.GetActiveScene().name == "Level_1" || isGrounded))
        {
            if (SceneManager.GetActiveScene().name == "Level_1" && dialougeRan == false)
            {
                if (Time.time - lastClickTime < doubleClickThreshold)
                {
                    dialogueExposition.ShowDialogue(jumpDialogue);
                    dialougeRan = true;
                }

                lastClickTime = Time.time;
            }
            rb.linearVelocity = new Vector2(rb.linearVelocity.x, jumpForce);
        }
    }
}
