using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public Transform player;  // Reference to the player's transform
    public Vector3 offset = new Vector3(0f, 2f, -10f);  // Offset from the player
    public float smoothSpeed = 5f;  // Smoothness of the camera movement
    public float maxFollowY = 5f;   // The Y position where the camera stops following

    private Rigidbody2D playerRb;
    private bool isFollowing = true;  // Track if camera is following

    void Start()
    {
        if (player != null)
        {
            playerRb = player.GetComponent<Rigidbody2D>();
        }
    }

    void LateUpdate()
    {
        if (player == null) return;  // Ensure player exists before updating

        // Get the camera's top boundary in world space
        float cameraTopY = transform.position.y + Camera.main.orthographicSize;

        // Stop following if the player reaches maxFollowY
        if (player.position.y >= maxFollowY)
        {
            isFollowing = false;
        }
        else if (player.position.y < maxFollowY)
        {
            isFollowing = true;
        }

        // If still following, smoothly move the camera
        if (isFollowing)
        {
            Vector3 targetPosition = player.position + offset;
            transform.position = Vector3.Lerp(transform.position, targetPosition, smoothSpeed * Time.deltaTime);
        }

        // Prevent the player from exceeding the camera's upper bound
        if (!isFollowing && player.position.y >= cameraTopY)
        {
            RestrictPlayerMovement(cameraTopY);
        }
    }

    void RestrictPlayerMovement(float cameraTopY)
    {
        if (playerRb != null)
        {
            // Stop the player from moving up further
            playerRb.linearVelocity = new Vector2(playerRb.linearVelocity.x, 0);
            player.position = new Vector3(player.position.x, cameraTopY, player.position.z);
        }
    }
}
